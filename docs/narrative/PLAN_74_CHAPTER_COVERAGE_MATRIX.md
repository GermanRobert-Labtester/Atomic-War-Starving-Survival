# Plan 74 — Chapter Coverage Matrix

Baseline audit of all fifteen entries in `narrative_progression.json` (five pre-existing, ten new under Plan 74).

| Order | ID | Title | Description theme | Trigger | Phase | Systems implied |
|---:|---|---|---|---|---|---|
| 1 | (none — description-embedded) | The Exchange | immediate catastrophe; detonations | static display (none) | early | world/history framing |
| 2 | (none) | Ashfall | surviving fallout and radiation | static display (none) | early | radiation, hazards |
| 3 | (none) | The Bunker | establishing shelter and community | static display (none) | early | shelter, survivors |
| 4 | (none) | First Contact | encountering other survivors | static display (none) | early | expeditions, NPCs |
| 5 | (none) | The Long Winter | nuclear winter conditions setting in | static display (none) | early/mid | weather, seasons |
| 6 | (none) | The Consolidation | factions become durable institutions; roads acquire owners | static display (none) | mid | faction territory (44), patrols, caravans |
| 7 | (none) | The Long Dark | deep-winter attrition of morale, sleep, people | static display (none) | mid | morale, guilt (66), shelter wear |
| 8 | (none) | The Thaw | mobility returns; routes reopen with strangers | static display (none) | mid | expeditions, weather gates (48), caravans |
| 9 | (none) | The Schism | alliances harden and split | static display (none) | mid | factions, beliefs (30C), treaties |
| 10 | (none) | The Black Market | unofficial economy matures | static display (none) | mid/late | trade (61), rare goods |
| 11 | (none) | The Reckoning | accumulated debts and guilt return | static display (none) | late | debt, guilt (66), faction memory |
| 12 | (none) | The Rebuilding | repair becomes durable construction | static display (none) | late | power (71), water, rail, infrastructure |
| 13 | (none) | The Second Winter | renewed deep cold against a harder world | static display (none) | late | weather, warlord doctrines (63) |
| 14 | (none) | The Muster | the surviving gather to decide who speaks for them | static display (none) | late | coalition/census, politics |
| 15 | (none) | The Inheritance | legacy, succession, cost accounting | static display (none) | endgame | epilogue (15A), generational legacy |

## Duplicate-resolution record (Task 74G)

The original draft proposed "First Winter" and "Long Dark" as new chapters. Audit found:

- **Existing Chapter 5 "The Long Winter" already covers winter onset.** The proposed "First Winter" chapter was therefore **dropped** (would duplicate Chapter 5).
- "The Long Dark" was **retained but repositioned** as the deep-winter interior phase (attrition, not onset) — distinct narrative function.
- Campaign never reaching a second winter is not a concern: the authoritative season calendar (`weather_seasons.json`) defines **High Cold at day 240** and **The Turning at day 300**, so a second deep-cold phase and a legacy phase are structurally reachable.
- Slot 14 uses **The Muster** (from the replacement pool) rather than a second scarcity chapter, because late-game already has The Reckoning and The Black Market covering pressure; the arc needed a political/opportunity beat between The Second Winter and The Inheritance.

## Status-prefix convention

Pre-existing entries embed `Complete` / `Active` / `Pending` in the description text. This is static authored text (see runtime contract). New chapters 6–15 follow the same convention with `Pending`, matching chapters 4–5.