# Plan 127 — Verdict Data Corruption Corpus & World History Ladder Expansion (8 → 25 corpus, 6 → 12 ladder)

## Goal (2 lines)
Expand the `corruption_corpus` (8 → 25) and `world_history_ladder` (6 → 12)
arrays in `verdict_data.json`. The Verdict expansion's master data
(`EvidenceLedger.cs` and `MachineLogSystem.cs` confirmed live) defines the
machine-counting-house theme: corruption_corpus is the garbled transmissions
the machine emits as it degrades, and world_history_ladder is the layered
discovery narrative the player ascends. Both are thin.

## Why (P2)
- Verified: `verdict_data.json` has `corruption_corpus` (8 strings),
  `world_history_ladder` (6 entries with layer, knowledge_key, title,
  discovery_location_id, body_summary), `currencies` (1), `readout_steps`
  (4), `facets` (3), `endings` (3). `EvidenceLedger.cs` and
  `MachineLogSystem.cs` consume the corpus and ladder.
- The corruption_corpus is the atmospheric text that makes the Verdict
  machine feel like it's breaking down — 8 garbled lines is too few to
  sustain the theme. The world_history_ladder is the discovery narrative
  — 6 layers is too short for a full expansion's lore arc.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/verdict_data.json` (expand
  `corruption_corpus` 8 → 25, `world_history_ladder` 6 → 12)
- Read-only: `Assets/Ashfall.Core/Verdict/EvidenceLedger.cs` (confirm how
  corpus strings are consumed)
- Read-only: `Assets/Ashfall.Core/Verdict/MachineLogSystem.cs` (confirm how
  ladder entries are consumed and how discovery_location_id resolves)

## Content grammar (per corruption_corpus entry)
- Plain string. The garbled, degraded transmission text the machine emits.
- Should feel like corrupted log output: repeated fragments, timestamps,
  garbled counts, partial words.

## Content grammar (per world_history_ladder entry)
- `layer`: integer, ascending (1, 2, 3, ...).
- `knowledge_key`: a lore key the ladder unlocks (must resolve against the
  knowledge catalog — confirm in step 2).
- `title`: evocative layer title.
- `discovery_location_id`: a location id where this layer is discovered
  (must resolve).
- `body_summary`: 2–4 sentences of lore prose for this layer.

## Steps
1. Read `EvidenceLedger.cs` to confirm how `corruption_corpus` strings are
   consumed (displayed verbatim? parsed for fragments?).
2. Read `MachineLogSystem.cs` to confirm how `world_history_ladder`
   entries are consumed, how `knowledge_key` resolves, and how
   `discovery_location_id` resolves.
3. Inventory the 8 existing corpus strings and 6 ladder entries. Confirm
   the quality bar and the corruption-text voice.
4. Author 17 new corruption_corpus strings:
   - Repeated count fragments ("the count is the count is the count is
     —").
   - Timestamped garble ("[03:14:00] — sector [unreadable] holds at
     [unreadable]").
   - Partial machine self-diagnosis ("valve. valve. the valve does not
     —").
   - Census window corruption ("CENSUS WINDOW: persons present: [garbled]
     [garbled] [garbled]").
   - Archive access failures ("the archive does not require — the
     archive does not —").
   - Signal loss patterns ("— signal lost mid-verbose. — signal lost
     mid-verbose. —").
   - Meter read loops ("the meter read. the meter — the meter —").
   - Hand/valve failures ("no hand on the valve. no hand — no hand —").
   - Count halts ("sector halts. sector — sector halts.").
   - And 8 more in the same degraded-machine voice.
5. Author 6 new world_history_ladder entries (layers 7–12):
   - Layer 7: the second geophone pit — deeper, older, a different
     machine.
   - Layer 8: the cable run east — what the machines were built to serve.
   - Layer 9: the counting house origin — why the count started.
   - Layer 10: the first halt — when the machine stopped and restarted.
   - Layer 11: the human hand — the last operator who touched the valve.
   - Layer 12: the open count — what happens when the count is never
     closed.
6. Each ladder entry: ascending layer, distinct knowledge_key (must
   resolve), distinct discovery_location_id (must resolve), 2–4 sentence
   body_summary in the Verdict voice.
7. Cross-reference: every knowledge_key resolves; every
   discovery_location_id resolves; every layer is unique and ascending.
8. Wire 3 new ladder entries to Plan 113 (Verdict questlines — questlines
   reference ladder discoveries).
9. Wire 2 new ladder entries to Plan 116 (deep lore locations — ladder
   discovery sites are deep lore locations).
10. Validate: `--data-integrity-selftest` (all knowledge_keys and
    discovery_location_ids resolve).
11. xUnit: verdict data loads 25 corpus strings + 12 ladder entries, all
    knowledge_keys resolving, all discovery_location_ids resolving, all
    layers ascending and unique.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data. The traps are `knowledge_key` and `discovery_location_id`
resolution (step 7): both must resolve against their respective catalogs.
Confirm the knowledge catalog and location catalog have the ids you plan
to reference, or use existing ids.

## Definition of Done
- `verdict_data.json` has 25 corruption_corpus strings + 12
  world_history_ladder entries, all knowledge_keys and
  discovery_location_ids resolving, all layers ascending, 3 wired to
  Verdict questlines, 2 to deep lore locations, integrity + tests green.

## Follow-on
- Plan 113 (Verdict questlines) — questlines reference ladder discoveries.
- Plan 116 (deep lore locations) — ladder sites are deep lore locations.
- Plan 94 (Verdict radio) — corpus strings complement radio broadcasts.
- Plan 82 (Verdict locations) — ladder discovery sites.
- Plan 89 (epilogues) — ladder completion feeds Verdict endings.
