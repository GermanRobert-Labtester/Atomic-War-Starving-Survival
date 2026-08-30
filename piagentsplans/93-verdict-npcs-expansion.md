# Plan 93 — Verdict NPCs Expansion (6 → 15 investigation-site NPCs)

## Goal (2 lines)
Expand `verdict_npcs.json` from 6 verified NPCs to 15. The Verdict NPC system
(`VerdictNpcSystem.cs` confirmed live) defines NPCs encountered at Verdict
investigation sites — each has a role, kind, gating flag, location, phase
minimum, and dialogue lines. 6 NPCs is too few for 15 investigation sites
(Plan 82); each site should have at least one NPC encounter.

## Why (P2)
- Verified: `verdict_npcs.json` has 6 entries (id, name, role, kind,
  gating_flag, location_id, phase_min, dialogue). `VerdictNpcSystem.cs` and
  `VerdictSave.cs` are confirmed live.
- Creates the investigation-NPC pillar: Verdict NPCs are the human element of
  investigation sites — an amateur radio operator, a fire-control engineer, a
  census clerk. They provide testimony, context, and mystery. 6 NPCs serves
  the existing 4 sites; 15 NPCs serves the expanded 15 sites from Plan 82.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/verdict_npcs.json` (expand 6 → 15 NPCs)
- Read-only: `Assets/Ashfall.Core/Verdict/VerdictNpcSystem.cs` (confirm schema
  and how gating_flag, location_id, and phase_min gate NPC availability)
- `Assets/StreamingAssets/Data/verdict_locations.json` (location_id must
  resolve — expanded by Plan 82)

## Content grammar (per NPC)
- snake_case `id` with prefix `npc_` (confirmed prefix).
- name: full name (Eden Vale, Ferris Voss) — these are named characters, not
  generic NPCs.
- role: 1 sentence describing the NPC's occupation and context ("Amateur radio
  operator, comm-array bleed").
- kind: NPC type (tape_echo, paper_ghost, and others — confirm accepted kinds
  by reading the existing entries).
- gating_flag: a `flag_*` id that must be set before the NPC appears (gates
  NPC availability by investigation progress).
- location_id: must resolve to an existing `loc_` id in verdict_locations.json.
- phase_min: investigation phase minimum (1, 2, 3 — gates by investigation
  depth).
- dialogue: array of 2–4 dialogue lines in the NPC's voice. Match the
  existing quality — terse, specific, slightly uncanny. These are people who
  have been alone too long.

## Steps
1. Read `VerdictNpcSystem.cs` to confirm the schema and how gating_flag,
   location_id, and phase_min gate NPC availability.
2. Read the existing 6 NPCs to confirm the quality bar and accepted `kind`
   values (tape_echo, paper_ghost — are there others?).
3. Confirm which `loc_` ids exist in verdict_locations.json (Plan 82 expands
   this to 15 sites).
4. Author 9 new NPCs, one for each new Verdict investigation site from Plan 82:
   - Coastal Survey arc (4): tide gauge keeper, meteorological station
     observer, cliff bunker signalman, marine lab researcher.
   - Interior Caches arc (4): forestry surveyor, geological core-sample
     technician, river gauge attendant, agricultural station botanist.
   - Border Wire arc (1): border relay operator.
5. Each NPC: distinct name, role, kind, gating_flag, location_id, phase_min,
   and 2–4 dialogue lines. Match the existing terse, uncanny tone.
6. Cross-reference: every npc id unique; every location_id resolves to a
  Verdict site; every gating_flag follows existing flag conventions; every
  kind is an accepted value.
7. Wire 3 NPCs into Plan 84 muster witnesses (Verdict NPCs can also serve as
   witnesses in the testimony network).
8. Validate: `--data-integrity-selftest` (all ids resolve).
9. xUnit: Verdict NPC catalog loads 15 NPCs, all ids unique, all location_id
   resolve, all gating_flag non-empty, all dialogue arrays non-empty.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data. The one trap is `kind` values (step 2): confirm accepted NPC
kinds before authoring — if only tape_echo and paper_ghost exist, either use
those or confirm new kinds are accepted by the system.

## Definition of Done
- `verdict_npcs.json` has 15 NPCs, all ids resolving, all location_id
  resolving to Verdict sites, 3 wired to muster witnesses, integrity + tests
  green.

## Follow-on
- Plan 82 (Verdict locations) — NPCs are site-linked.
- Plan 84 (muster witnesses) — 3 NPCs double as witnesses.
- Plan 94 (Verdict radio) — NPCs reference radio broadcasts.
- Plan 52 (recurring NPC arcs) — Verdict NPCs can recur.
- Existing 18 (expansion deepening) — this plan deepens the Verdict expansion.
