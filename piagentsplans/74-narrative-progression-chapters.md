# Plan 74 — Narrative Progression Chapters Expansion (5 → 15 campaign chapters)

## Goal (2 lines)
Expand `narrative_progression.json` from 5 verified chapter entries to 15. The narrative
progression system defines the campaign's chapter structure — each chapter has a
description and order, marking a major campaign phase. 5 chapters is too few for a full
campaign arc; 15 creates a visible narrative spine from the exchange through long-term
society-building.

## Why (P2)
- Verified: `narrative_progression.json` has 5 entries (description, order). The system
  defines the campaign's narrative arc — Chapter 1 (The Exchange), Chapter 2 (Ashfall),
  Chapter 3 (The Bunker), and 2 others. 5 chapters covers the early game but not the
  mid-game faction consolidation or late-game society-building.
- Creates the campaign-spine pillar: chapters are the visible structure of the campaign
  — they mark when the world shifts from one phase to another (confusion → survival →
  consolidation → conflict → rebuilding → generational). The player should feel the
  campaign has a shape, not just an endless survival loop.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/narrative_progression.json` (expand 5 → 15 chapters)
- Read-only: confirm the progression system consumer — `grep -rn "narrative_progression\|NarrativeProgression\|chapter"
  Assets/Ashfall.Core/` to find the loader and confirm how chapters trigger (day
  threshold, flag, story event)

## Content grammar (per chapter)
- order: integer (1–15) — the chapter's position in the campaign.
- title: 2-5 words, evocative (The Exchange, Ashfall, The Bunker, The Consolidation,
  The Schism, etc.).
- description: 1-2 sentences describing the chapter's theme and what changes in the
  world. Grounded, not expository. Skill `ashfall-write`.
- trigger_day: approximate day the chapter begins (early: 1–30, mid: 31–100, late:
  101+).
- phase: early / mid / late — the campaign phase.
- world_state_changes: what changes in this chapter (faction consolidation, resource
  depletion, territory shifts, new threat types, new content unlocks — feeds Plan 57
  incidents, Plan 44 territory, Plan 63 warlord doctrines).

## Steps
1. Find the progression system consumer to confirm the schema and how chapters trigger.
2. Read the 5 existing chapters to understand the structure and tone.
3. Author 10 new chapters, continuing the campaign arc:
   - Chapter 6: The Consolidation (day ~35) — factions begin to organize, territory
     lines harden, the player must choose sides or stay independent.
   - Chapter 7: The First Winter (day ~50) — severe cold, fuel crisis, the shelter is
     tested for the first time (feeds Plan 48 weather gates, Plan 70 winter schedule).
   - Chapter 8: The Long Dark (day ~65) — morale crisis, the longest winter, survivors
     question whether survival is worth it (feeds Plan 66 guilt, Plan 68 wall carvings).
   - Chapter 9: The Thaw (day ~80) — spring brings movement, expeditions open, caravans
     resume, but so do raids (feeds Plan 60 vehicles, Plan 45 patrols).
   - Chapter 10: The Schism (day ~95) — a major faction splits; the player's
     relationships determine which side they're on (feeds existing 30C belief movements).
   - Chapter 11: The Black Market (day ~110) — a shadow economy emerges; rare goods
     become available through smugglers (feeds Plan 61 trade scenarios).
   - Chapter 12: The Reckoning (day ~125) — old debts come due; the player's past
     choices catch up (feeds Plan 40 debt, Plan 66 guilt).
   - Chapter 13: The Rebuilding (day ~140) — infrastructure projects become possible;
     the player can repair rail, power, water systems (feeds Plan 55 recipes, Plan 71
     power grid).
   - Chapter 14: The Second Winter (day ~160) — the shelter is stronger, but the world
     is more dangerous; factions are desperate (feeds Plan 63 warlord doctrines).
   - Chapter 15: The Inheritance (day ~180+) — the campaign's endgame; the player's
     legacy is determined by accumulated choices (feeds existing 15A epilogue).
4. Give each chapter: order, title, description, trigger_day, phase, world_state_changes.
5. Wire 5 chapters to Plan 57 incidents (chapter transitions trigger shelter incidents
   that mark the shift).
6. Wire 3 chapters to Plan 44 faction territory (consolidation, schism, reckoning shift
   territory control).
7. Wire 3 chapters to existing 19C seasonal cadence (first winter, thaw, second winter).
8. Validate: `--data-integrity-selftest`; confirm chapters advance on the correct day
   in a headless boot; confirm world_state_changes fire.
9. xUnit: progression catalog loads, all chapters in order, trigger_day advances
   chapters, world_state_changes fire, save round-trip preserves current chapter.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data + narrative authoring.

## Definition of Done
- `narrative_progression.json` has 15 chapters (5 existing + 10 new), all in order,
  trigger_day advances chapters, 5 wired to incidents, 3 wired to territory, 3 wired
  to seasons, world_state_changes fire, save round-trip green, integrity + tests green.

## Follow-on
- Plan 57 (incidents) — chapter transitions trigger shelter incidents.
- Plan 44 (faction territory) — consolidation, schism, reckoning shift territory.
- Existing 19C (seasonal cadence) — chapters align with seasonal shifts.
- Existing 15A (epilogue chronicle) — the final chapter feeds the epilogue.
- Plan 63 (warlord doctrines) — late chapters escalate warlord activity.
- Plan 66 (guilt) — the reckoning chapter brings past guilt to the surface.
