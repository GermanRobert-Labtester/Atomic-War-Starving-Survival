# Plan 92 — Faction War Dialogue Expansion (18 → 40 overheard conversation snippets)

## Goal (2 lines)
Expand `faction_war_dialogue.json` from 18 verified snippets to 40. The faction
war dialogue system (`FactionWarContentCatalog.cs` confirmed live) defines
overheard conversations the player encounters at faction locations — each
snippet has a location, minimum day, speaker tag, and body. 18 snippets is
too few for a world with multiple factions across a 300+ day campaign.

## Why (P2)
- Verified: `faction_war_dialogue.json` has 18 entries (id, locationId, minDay,
  speakerTag, body). `FactionWarContentCatalog.cs` is confirmed in Core.
- Creates the overheard-world pillar: these snippets are how the player
  eavesdrops on the world — two quartermasters arguing about manifests, two
  traders betting on a silo, two relay hands striking a mast. They make
  factions feel like living organizations with internal conversations, not
  quest dispensers. 18 snippets is one faction's worth; 40 covers garrison,
  exchange, understory, and independent factions.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/faction_war_dialogue.json` (expand 18 → 40)
- Read-only: `Assets/Ashfall.Core/YearOfAsh/FactionWarContentCatalog.cs`
  (confirm schema and how locationId and minDay gate snippet availability)

## Content grammar (per snippet)
- snake_case `id` with prefix `dlg_` (confirmed prefix).
- locationId: must resolve to an existing `loc_` id (from locations.json,
  verdict_locations.json, or expedition destinations).
- minDay: the earliest day this snippet can be overheard (gates by campaign
  progress).
- speakerTag: 1 sentence describing who is talking and what they're doing
  ("Two Garrison quartermasters, reconciling manifests").
- body: 2–6 lines of dialogue. Match the existing quality — naturalistic,
  overheard, no exposition dumps. Characters talk about logistics, weather,
  rumors, petty grievances, not the plot. Use `\"` for quoted speech.
- Diversity: cover garrison, exchange, understory, independent, foundry, and
  civilian factions. Cover logistics, morale, rumors, personal disputes,
  resource shortages, and idle talk.

## Steps
1. Read `FactionWarContentCatalog.cs` to confirm the schema and how snippets
   are selected (by locationId + current day >= minDay?).
2. Read the existing 18 snippets to confirm the quality bar (the quartermaster
   and silo snippets are the model — naturalistic, funny, human).
3. Confirm which `loc_` ids exist across location catalogs for locationId
   references.
4. Author 22 new snippets across 6 faction contexts:
   - Garrison (5): supply dispute, patrol rotation complaint, officer gossip,
     sick-list argument, fuel rationing debate.
   - Exchange (4): price negotiation, caravan delay, trader's complaint about
     garrison tolls, water-queue incident.
   - Understory (4): relay maintenance, broadcast schedule argument, cipher
     confusion, antenna repair story.
   - Independent (3): scavenger territory dispute, camp defense plan, refugee
     admission argument.
   - Foundry (3): molders vs. apprentices, furnace schedule, iron quota
     dispute.
   - Civilian (3): two survivors sharing a meal, a parent and child, an old
     man talking to nobody.
5. Each snippet: distinct locationId, minDay, speakerTag, and body. Match
   the existing naturalistic tone — characters talk like real people, not
   quest NPCs.
6. Cross-reference: every snippet id unique; every locationId resolves to an
   existing location; minDay within valid campaign range.
7. Validate: `--data-integrity-selftest` (all ids resolve).
8. xUnit: faction war dialogue catalog loads 40 snippets, all ids unique,
   all locationId resolve, all body non-empty.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data. The one trap is locationId resolution (step 3): confirm the
location exists before authoring.

## Definition of Done
- `faction_war_dialogue.json` has 40 snippets, all ids resolving, all
  locationId resolving, integrity + tests green.

## Follow-on
- Plan 84 (muster witnesses) — witnesses and overheard dialogue both reveal
  world state.
- Plan 73 (faction radio) — radio broadcasts and overheard dialogue
  complement each other.
- Plan 44 (faction territory) — dialogue snippets are location-gated by
  faction territory.
- Plan 52 (recurring NPC arcs) — some speakers can be recurring NPCs.
- Existing 25 (faction ecology) — this plan provides the faction-dialogue
  data.
