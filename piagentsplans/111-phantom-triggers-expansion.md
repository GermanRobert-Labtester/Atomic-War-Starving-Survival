# Plan 111 — Phantom Memory Triggers Expansion (7 → 20 backgrounds)

## Goal (2 lines)
Expand `phantom_triggers.json` from 7 survivor backgrounds to 20. The
PhantomMemoryEngine (`PhantomMemoryEngine.cs` confirmed live) fires
motivation/breakdown events when a survivor with a given background encounters
a triggering item category (a childhood toy, a photograph, a uniform). 7
backgrounds cover only the starter roster; the expanded survivor pool needs
coverage for the occupations and histories the world actually contains.

## Why (P2)
- Verified: `phantom_triggers.json` has 7 `items` entries keyed by
  `background_id` (child_refugee, former_soldier, nurse, teacher,
  electrician, machinist, generic). Each has a `triggers` array of
  {item_category, motivation_chance, description, motivation_text,
  breakdown_text}. `PhantomMemoryEngine.cs` consumes this directly.
- The survivor roster includes backgrounds beyond these 7 (engineers,
  farmers, fishermen, clerics, drivers, medics, scavengers, etc.) — they
  currently fall through to `generic` and never get a personal phantom moment.
- Pure DATA work — zero new Core code. The engine already keys by
  `background_id` and falls back to `generic`.

## Files to touch
- `Assets/StreamingAssets/Data/phantom_triggers.json` (expand `items` 7 → 20)
- Read-only: `Assets/Ashfall.Core/PhantomMemoryEngine.cs` (confirm background
  lookup and fallback behavior)
- Read-only: `Assets/Ashfall.Core/Phantoms/PhantomTriggerDto.cs` (confirm DTO)

## Content grammar (per background entry)
- `background_id`: snake_case string matching a survivor background id used
  by the survivor generation system.
- `triggers`: array of trigger objects:
  - `item_category`: an item category the engine matches against scavenged
    or carried items (e.g. "childhood", "photograph", "uniform",
    "medication", "tool", "weapon", "document", "food", "music").
  - `motivation_chance`: 0.0–1.0 probability the encounter produces a
    motivation rather than a breakdown.
  - `description`: the scene the player witnesses (second person, present
    tense, references {name}).
  - `motivation_text`: the positive outcome line ({name} steadies, resolves).
  - `breakdown_text`: the negative outcome line ({name} spirals, freezes).

## Steps
1. Read `PhantomMemoryEngine.cs` to confirm how `background_id` is matched
   against the survivor and how `item_category` is resolved (against item
   tags? item ids? a category field on the item?).
2. Read `PhantomTriggerDto.cs` to confirm the trigger field set and any
   validation (range on motivation_chance, required fields).
3. Inventory all survivor `background_id` values used in the survivor
   generation data (`survivors.json` or equivalent) to identify the 13
   backgrounds missing from the trigger file.
4. Author 13 new background entries:
   - `farmer`: triggers on seed, tool, food — the land they lost.
   - `fisherman`: triggers on rope, fish, water — the coast they fled.
   - `engineer`: triggers on tool, document, machinery — the thing they built
     that failed.
   - `driver`: triggers on vehicle_part, fuel, map — the road they didn't
     take.
   - `cleric`: triggers on religious_object, document, candle — the faith
     they question.
   - `medic`: triggers on medication, bandage, document — the patient they
     couldn't save.
   - `scavenger`: triggers on tool, weapon, container — the find that went
     wrong.
   - `cook`: triggers on food, fuel, utensil — the meal that poisoned
     someone.
   - `radio_operator`: triggers on radio, battery, document — the signal
     they ignored.
   - `miner`: triggers on tool, lamp, rock — the collapse they walked away
     from.
   - `librarian`: triggers on book, document, photograph — the archive they
     burned for warmth.
   - `carpenter`: triggers on wood, tool, furniture — the house they couldn't
     reinforce.
   - `chemist`: triggers on chemical, medication, document — the formula
     they misread.
5. Each background: 3–5 triggers across distinct item categories, each with
   a motivation_chance (0.2–0.6), and distinct description/motivation/
   breakdown prose in the established second-person voice.
6. Ensure every `item_category` used matches a category the engine can
   actually resolve (cross-check step 1).
7. Wire 4 new backgrounds' breakdown outcomes into Plan 66 (guilt — phantom
   breakdowns generate guilt).
8. Wire 3 new backgrounds' motivation outcomes into Plan 33 (skills —
   motivation grants a temporary skill bonus).
9. Validate: `--data-integrity-selftest` (loads cleanly).
10. xUnit: phantom trigger catalog loads 20 backgrounds, all background_ids
    unique, all triggers have non-empty description/motivation/breakdown.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data. The one trap is `item_category` resolution (step 1): confirm
whether categories are item tags, ids, or a dedicated category field before
authoring triggers that will never fire.

## Definition of Done
- `phantom_triggers.json` has 20 background entries, all background_ids
  unique, all item_categories resolvable, 4 wired to guilt, 3 to skills,
  integrity + tests green.

## Follow-on
- Plan 66 (guilt) — phantom breakdowns generate guilt.
- Plan 33 (skills) — motivation grants temporary skill bonus.
- Plan 95 (journal voice) — phantom moments trigger journal entries.
- Plan 88 (confessions) — breakdown survivors may confess.
- Plan 109 (echo quests) — a phantom breakdown may trigger a later echo.
