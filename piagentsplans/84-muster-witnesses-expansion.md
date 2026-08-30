# Plan 84 — Muster Witness Testimonies Expansion (3 → 15 witnesses)

## Goal (2 lines)
Expand `muster_witnesses.json` from 3 verified witnesses to 15. The Muster
witness system (`WitnessCatalog.cs` confirmed live) defines NPC testimonies the
player collects — each witness has a name, location, knowledge key, minimum day,
and a body of testimony text. The existing 3 witnesses form a single
investigation thread (the Voss disappearance); 15 witnesses creates a
multi-thread testimony network.

## Why (P2)
- Verified: `muster_witnesses.json` has 3 entries (id, witness_name, location_id,
  knowledge_key, day_min, body). `WitnessCatalog.cs` is confirmed in Core.
  `JournalWitnessPanel.cs` is confirmed in the Godot host. The existing 3
  witnesses are richly written (checkpoint conscript, quartermaster, signals
  sergeant) but all serve one mystery.
- Creates the testimony-network pillar: witnesses are how the player learns
  what happened in the world — not through exposition, but through conflicting
  accounts from people who were there. 15 witnesses across 3–4 investigation
  threads creates a web of contradictory testimony the player must reconcile.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/muster_witnesses.json` (expand 3 → 15 witnesses)
- Read-only: `Assets/Ashfall.Core/Muster/WitnessCatalog.cs` (confirm schema and
  how knowledge_key and location_id resolve)
- `Assets/StreamingAssets/Data/locations.json` or `verdict_locations.json`
  (location_id must resolve to an existing location)

## Content grammar (per witness)
- snake_case `id` with prefix `witness_` (confirmed prefix).
- witness_name: a named character or a role ("The Checkpoint Conscript",
  "Quartermaster Voss", "Signals Sergeant Anneke Ruhl"). Mix named and unnamed
  witnesses — some survivors are known, others are encountered by role.
- location_id: must resolve to an existing `loc_` id (from locations.json,
  verdict_locations.json, or expedition destinations).
- knowledge_key: a `history_*` or `knowledge_*` id that the testimony unlocks.
- day_min: the earliest day this witness can be encountered (gates testimony
  availability by campaign progress).
- body: 2–5 sentences of testimony in the witness's voice. Match the existing
  quality — grounded, specific, contradictory, human. Each witness should
  reveal something the others don't, or contradict another witness.
- Investigation threads: group witnesses into 3–4 threads. The existing 3 form
  the "Voss Disappearance" thread; add threads for other mysteries.

## Steps
1. Read `WitnessCatalog.cs` to confirm the schema and how knowledge_key and
   location_id resolve (does the system check that location_id exists in a
   location catalog?).
2. Read the existing 3 witnesses to confirm the quality bar and the Voss
   thread's structure (witnesses contradict each other — Voss was shot vs.
   Voss was reassigned vs. Voss's cipher was intercepted).
3. Confirm which `loc_` ids exist across locations.json, verdict_locations.json,
   and expeditions.json for location_id references.
4. Author 12 new witnesses in 3 new investigation threads:
   - Thread "The Coastal Evacuation" (4 witnesses): harbor master, fishing boat
     captain, refugee camp nurse, naval conscript. What happened to the
     evacuation fleet? Conflicting accounts: it left, it was sunk, it never
     sailed.
   - Thread "The Grain Convoy Massacre" (4 witnesses): convoy driver, Rebuilder
     medic, garrison soldier, civilian witness. Who fired first? Each witness
     blames a different faction.
   - Thread "The Silent Foundry Accord" (4 witnesses): foundry molder, ice-road
     hauler, Office clerk, Cluster elder. Was the brine-pipe treaty honored?
     Each witness has a different interest.
5. Each witness: distinct voice, specific detail, and a piece of information
   that contradicts or complicates at least one other witness in the same
   thread. The player must decide which account to believe.
6. Cross-reference: every witness id unique; every location_id resolves to an
   existing location; every knowledge_key follows existing conventions.
7. Wire 3 witnesses to Plan 82 Verdict investigation sites (witnesses at coastal
   and border sites provide testimony about those locations).
8. Validate: `--data-integrity-selftest` (all ids resolve).
9. xUnit: witness catalog loads 15 witnesses, all ids unique, all location_id
   resolve, all knowledge_key non-empty, day_min within valid campaign range.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data. The one trap is location_id resolution (step 6): confirm the
location_id exists in one of the location catalogs before authoring.

## Definition of Done
- `muster_witnesses.json` has 15 witnesses in 4 investigation threads, all ids
  resolving, 3 wired to Verdict sites, integrity + tests green.

## Follow-on
- Plan 82 (Verdict locations) — witnesses at investigation sites provide
  testimony about those sites.
- Plan 52 (recurring NPC arcs) — named witnesses can recur as NPCs.
- Plan 51 (environmental storytelling) — witness testimony cross-references
  found documents.
- Plan 73 (faction radio) — intercepted broadcasts corroborate or contradict
  witness testimony.
- Existing 25 (faction ecology) — witnesses reveal faction behavior.
