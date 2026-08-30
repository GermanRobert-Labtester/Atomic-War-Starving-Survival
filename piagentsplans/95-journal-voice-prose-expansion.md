# Plan 95 — Journal Voice Prose Expansion (expand personality-variant prose)

## Goal (2 lines)
Expand `journal_voice_prose.json` with more situation keys and more personality
variants per key. The journal voice system (`JournalVoice.cs` and
`JournalVoiceProseCatalog.cs` confirmed live) defines personality-variant prose
for journal entries — each situation key (high_co2, has_seen_radiation,
has_experienced_storm) has variants for 7 personality types (default, paranoid,
cautious, realist, reckless, denialist, fatalist). The existing catalog has
too few situation keys.

## Why (P2)
- Verified: `journal_voice_prose.json` has a `prose_variants` object with
  situation keys, each containing 7 personality variants (default, paranoid,
  cautious, realist, reckless, denialist, fatalist).
  `JournalVoice.cs` and `JournalVoiceProseCatalog.cs` are confirmed live.
- Creates the journal-voice pillar: journal entries should sound different
  depending on who's writing — a paranoid survivor sees conspiracy in a CO2
  spike; a fatalist sees inevitability; a denialist sees nothing. More
  situation keys mean more journal entries have personality, not just
  generic text.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/journal_voice_prose.json` (add situation keys)
- Read-only: `Assets/Ashfall.Core/Journal/JournalVoice.cs`,
  `Assets/Ashfall.Core/Journal/JournalVoiceProseCatalog.cs` (confirm how
  situation keys are selected and how personality variants are applied)

## Content grammar (per situation key)
- Key name: snake_case describing the situation (high_co2, has_seen_radiation,
  has_experienced_storm, low_food, death_of_survivor, faction_raid,
  successful_expedition, etc.).
- 7 personality variants per key: default, paranoid, cautious, realist,
  reckless, denialist, fatalist. Each is 1–2 sentences in that personality's
  voice.
- Personality voice rules:
  - default: neutral, factual.
  - paranoid: suspicious, conspiratorial, sees threat everywhere.
  - cautious: careful, measured, focuses on risk mitigation.
  - realist: pragmatic, data-focused, no emotion.
  - reckless: dismissive of danger, action-oriented.
  - denialist: refuses to acknowledge the problem.
  - fatalist: accepts doom as inevitable, darkly resigned.
- Each variant should be distinct — no two personalities should say the same
  thing in different words.

## Steps
1. Read `JournalVoice.cs` and `JournalVoiceProseCatalog.cs` to confirm how
   situation keys are selected (by game state? by event trigger?) and how
   personality variants are chosen (by survivor personality trait?).
2. Read the existing situation keys (high_co2, has_seen_radiation,
   has_experienced_storm, and any others) to confirm the quality bar and
   the 7-variant pattern.
3. Author 12 new situation keys:
   - `low_food`: food shortage journal entry.
   - `low_water`: water shortage journal entry.
   - `death_of_survivor`: a survivor has died.
   - `successful_expedition`: an expedition returned with loot.
   - `failed_expedition`: an expedition returned empty or with casualties.
   - `faction_raid`: the shelter was raided.
   - `disease_outbreak`: a disease is spreading.
   - `power_failure`: the grid went down.
   - `new_survivor_arrived`: a new survivor joined the shelter.
   - `severe_cold`: extreme cold weather event.
   - `high_radiation_zone`: entered a high-radiation area.
   - `moral_compromise`: the player made a difficult moral choice.
4. Each key: 7 personality variants, each 1–2 sentences. Match the existing
   quality — each personality has a distinct voice and worldview.
5. Cross-reference: every situation key unique; every key has all 7
   personality variants; no two variants within a key are identical.
6. Validate: `--data-integrity-selftest` (all keys resolve).
7. xUnit: journal voice prose catalog loads all situation keys, each with 7
   non-empty variants, no duplicate variants within a key.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data. The one trap is situation-key selection (step 1): confirm
how the system chooses which key to use — if it's event-triggered, new keys
must correspond to events the system actually fires.

## Definition of Done
- `journal_voice_prose.json` has 12+ new situation keys, each with 7
  personality variants, all keys unique, all variants distinct, integrity +
  tests green.

## Follow-on
- Plan 88 (confession secrets) — confessions could trigger journal entries.
- Plan 66 (guilt sources) — guilt triggers moral_compromise journal entries.
- Plan 65 (final wishes) — a survivor's death triggers death_of_survivor.
- Plan 57 (incidents) — incidents trigger situation keys.
- Existing 27A (body and mind) — journal voice is the psychological
  expression layer.
