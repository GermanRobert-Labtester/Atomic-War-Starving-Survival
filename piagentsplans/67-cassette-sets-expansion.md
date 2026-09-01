# Plan 67 — Cassette Sets Expansion (4 → 12 multi-part audio narratives)

## Goal (2 lines)
Expand `cassette_sets.json` from 4 verified entries to 12. Each cassette set is a
multi-part audio narrative the player finds and plays in sequence — a pre-war story told
through found tapes. The cassette system feeds the `VinylMoraleSystem` and journal
unlocks. 4 sets is too few for a full campaign of discovery.

## Why (P2)
- Verified: `cassette_sets.json` has 4 entries (set_id, set_title, total_parts, parts
  with part number and content). Existing 06B planned echoes/cassettes 23 → 40 but the
  cassette set catalog itself is thin.
- Creates the audio-discovery pillar: cassette sets are long-form environmental
  storytelling — each set tells a complete pre-war story (a family's last days, a
  military unit's descent, a radio operator's logs, a scientist's final recordings).
  Finding all parts of a set is a scavenging motivation.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/cassette_sets.json` (expand 4 → 12 sets)
- `Assets/StreamingAssets/Data/items.json` (add `item_cassette_*` entries for each part)
- Read-only: confirm the cassette system consumer — `grep -rn "cassette\|Cassette"
  Assets/Ashfall.Core/` to find the loader and confirm the schema

## Content grammar (per cassette set)
- snake_case `id` with prefix `cassette_` or `set_` (confirm from existing 4 entries —
  they use `set_id` like `checkpoint_kilo`).
- set_title: evocative, 3-6 words.
- total_parts: 3-6 parts per set.
- parts: each part has: part number, content (the audio transcript — 1-3 paragraphs of
  grounded, human prose), location_hint (where this part is found — feeds Plan 46
  scavenging tables), journal_unlock (optional `journal_*` id — listening to all parts
  unlocks a journal entry with the full story).
- tone: cold, exhausted, human, restrained (per AGENTS.md). The tapes are pre-war, so
  the tone can be slightly more alive — but still grounded. Skill `ashfall-write`.

## Steps
1. Find the cassette system consumer to confirm the schema and how parts resolve.
2. Read the 4 existing sets to understand the structure and avoid duplication.
3. Author 8 new cassette sets, each 3-6 parts:
   - "The Last Days of Checkpoint Kilo" (existing — do not duplicate).
   - "Field Hospital 7" (5 parts: a nurse's audio logs from a military field hospital
     in the days before the exchange — feeds Plan 112 disease content and Plan 09A medical response).
   - "The Evacuation Train" (4 parts: a conductor's recordings as the last train leaves
     the city — feeds Plan 49 frozen evacuation bus).
   - "Station 14" (6 parts: a radio operator's broadcasts as the world ends — feeds
     existing 24A radio).
   - "The Greenhouse Tapes" (3 parts: a botanist's recordings from an agricultural
     research station — feeds Plan 91 greenhouse).
   - "Father's Tapes" (4 parts: a father recording messages for his child, found in
     an apartment — feeds Plan 51 environmental documents).
   - "The Dam Keeper's Log" (5 parts: a hydroelectric dam operator's recordings as the
     grid fails — feeds Plan 71 power grid).
   - "The Teacher's Recordings" (3 parts: a schoolteacher reading lessons to an empty
     classroom — feeds Plan 47 collectibles).
   - "The Quarantine Tapes" (4 parts: a doctor's recordings during a disease outbreak
     before the exchange — feeds Plan 112 disease content and Plan 09A response).
4. Write each part's content in ASHFALL tone. Each part is 1-3 paragraphs — a fragment
   of a story that only makes sense when all parts are found and played.
5. Add `item_cassette_*` entries to `items.json` for each part (so they're lootable).
6. Wire 6 sets into Plan 46 scavenging tables — each part appears in a specific
   location type (hospital tapes in hospitals, school tapes in schools, etc.).
7. Wire 4 sets to journal unlocks — listening to all parts unlocks a journal entry
   with the complete story (feeds existing 17C codex).
8. Cross-reference: every `item_cassette_*` id resolves; every `journal_*` unlock
   resolves; every location_hint references a valid scavenging table.
9. Validate: `--data-integrity-selftest`; confirm a cassette part is found via
   scavenging, played, and the journal unlock fires when all parts are collected in a
   headless boot.
10. xUnit: cassette catalog loads, all references resolve, parts play in sequence,
    journal unlocks fire on complete sets, save round-trip preserves collected parts.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data + narrative authoring.

## Definition of Done
- `cassette_sets.json` has 12 sets (4 existing + 8 new), all parts have `item_*` entries,
  6 wired to scavenging tables, 4 wired to journal unlocks, parts play in sequence,
  journal unlocks fire on complete sets, save round-trip green, integrity + tests green.

## Follow-on
- Plan 46 (scavenging) — cassette parts are location-specific loot.
- Plan 51 (documents) — cassette sets overlap with environmental storytelling.
- Existing 06B (echoes/cassettes 23 → 40) — this plan expands the cassette set catalog.
- Existing 17C (codex) — complete sets unlock journal entries.
- Plan 47 (collectibles) — cassette sets are a collectible category.
