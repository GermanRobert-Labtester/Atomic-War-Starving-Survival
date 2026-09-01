# Plan 105 — Trade Specialties Expansion (4 → 12 profession trade specialties)

## Goal (2 lines)
Expand `trade_specialties.json` from 4 verified professions to 12. The trade
specialty system (`TradeSpecialtySystem.cs` confirmed live) defines
profession-based trade milestones — each profession has 3 tiers with item
patterns, titles, narrative ids, skill bonuses, and mastery text. 4
professions (electrician, nurse, machinist, teacher) is too few for a
survivor-roster with diverse occupations.

## Why (P2)
- Verified: `trade_specialties.json` has 4 entries (profession_id,
  display_name, milestones with tier, item_patterns, title, narrative,
  skill_bonus, mastery_narrative, mastery_bonus_text).
  `TradeSpecialtySystem.cs` is confirmed in Core.
- Creates the profession-progression pillar: trade specialties are how
  survivors advance in their pre-war professions — each tier unlocks better
  trade options for related items. 4 professions covers 4 skill domains; 12
  covers the full range of survivor occupations (medical, technical,
  military, agricultural, domestic, scientific, social, creative).
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/trade_specialties.json` (expand 4 → 12)
- Read-only: `Assets/Ashfall.Core/Survivors/TradeSpecialtySystem.cs` (confirm
  schema and how profession_id, item_patterns, and skill_bonus are applied)

## Content grammar (per profession)
- snake_case `profession_id` (e.g. electrician, nurse, machinist, teacher).
- `display_name`: profession name (Electrician, Nurse, Machinist, Teacher).
- `milestones`: 3 tiers (1=apprentice, 2=journeyman, 3=master):
  - `tier`: integer 1–3.
  - `item_patterns`: array of keyword patterns that match item ids (e.g.
    "battery", "generator", "circuit" for electrician tier 1).
  - `title`: milestone title ("Apprentice Spark", "Journeyman Current",
    "Master of the Grid").
  - `narrative`: narrative event id (narrative_* prefix).
  - `skill_bonus`: float (0.05 = 5% bonus to trade value for matching items).
- `mastery_narrative`: narrative event id for mastery.
- `mastery_bonus_text`: 1–2 sentences of prose describing mastery, using
  `{name}` placeholder for the survivor's name. Match the existing quality.
- Each profession should have distinct item_patterns that don't overlap
  with other professions.

## Steps
1. Read `TradeSpecialtySystem.cs` to confirm the schema and how
   profession_id, item_patterns, and skill_bonus are applied (does the
   system match item ids by substring? by pattern?).
2. Read the existing 4 professions to confirm the quality bar and the 3-tier
   milestone structure.
3. Author 8 new professions:
   - `farmer`: agricultural items (seed, soil, fertilizer, irrigation,
     harvest). Titles: Hand Planter, Field Steward, Master of the Harvest.
   - `soldier`: military items (weapon, ammo, armor, tactical, combat).
     Titles: Raw Recruit, Field Corporal, Master of the Line.
   - `cook`: food items (food, ration, spice, cook, kitchen). Titles: Mess
     Hand, Camp Cook, Master of the Galley.
   - `carpenter`: construction items (wood, nail, timber, beam, shelter).
     Titles: Bench Hand, Frame Maker, Master of the Frame.
   - `doctor`: advanced medical items (surgical, pharmaceutical, diagnostic,
     anesthesia). Titles: Intern, Attending, Master Physician.
   - `scientist`: research items (sample, data, instrument, analysis,
     reagent). Titles: Lab Assistant, Research Fellow, Master of the Method.
   - `priest`: social/spiritual items (book, candle, relic, shrine, prayer).
     Titles: Novitiate, Pastor, Master of the Parish.
   - `hunter`: wilderness items (trap, pelt, game, trail, carcass). Titles:
     Trail Hand, Woodsman, Master of the Hunt.
4. Each profession: 3 tiers with distinct item_patterns, titles, narratives,
   and skill bonuses. Match the existing `{name}` placeholder convention.
5. Cross-reference: every profession_id unique; every item_pattern is a
   distinct keyword; every narrative id follows existing conventions; no two
   professions share the same item_patterns.
6. Wire 3 professions into Plan 33 (skill catalog — trade specialties grant
   skill bonuses).
7. Wire 2 professions into Plan 80 (library manuals — manuals grant
   profession-relevant skill XP).
8. Validate: `--data-integrity-selftest` (all ids resolve).
9. xUnit: trade specialty catalog loads 12 professions, all ids unique,
   all 3 tiers present, all item_patterns non-empty, all skill_bonus within
   valid range.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data. The one trap is item_pattern matching (step 1): confirm
whether patterns are substring matches, regex, or exact item-id matches
before authoring.

## Definition of Done
- `trade_specialties.json` has 12 professions, all ids resolving, 3 wired to
  skill catalog, 2 wired to library manuals, integrity + tests green.

## Follow-on
- Plan 33 (skill catalog) — trade specialties grant skill bonuses.
- Plan 80 (library manuals) — manuals grant profession-relevant XP.
- Plan 56 (economy goods) — profession item_patterns match trade goods.
- Plan 72 (utility AI actions) — profession determines survivor actions.
- Plan 52 (recurring NPC arcs) — profession shapes NPC behavior.
