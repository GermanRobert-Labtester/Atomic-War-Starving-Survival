# Plan 52 — Recurring NPC Arcs (36 → 60 characters with temporal arcs)

## Goal (2 lines)
Expand `characters.json` from 36 to 60 named NPCs, with the 24 new characters designed as
recurring figures with temporal arcs: a trader met on Day 15 reappears injured on Day 40
or becomes a faction official by Day 80. Each NPC has an occupation, skill, weakness,
personal objective, faction affiliation, and a state that evolves across the campaign.

## Why (P2)
- Verified: `characters.json` has 36 NPCs (id: `npc_*`); `survivors.json` has 129 survivor
  records but these are generic stat blocks, not named characters with arcs. The world
  has no recurring faces — NPCs are encountered once and forgotten.
- Creates the NPC-continuity pillar: the player builds relationships with named characters
  who remember past interactions, change over time, and reappear in new roles. A trader
  who was helped on Day 15 offers a discount on Day 40; a survivor who was refused shelter
  on Day 20 leads a raid on Day 60.
- Pure DATA work — extends the existing character catalog with arc-state fields.

## Files to touch
- `Assets/StreamingAssets/Data/characters.json` (expand 36 → 60)
- Read-only: `Assets/StreamingAssets/Data/factions.json` (19 factions — NPC affiliation
  must resolve), `Assets/StreamingAssets/Data/survivors.json` (129 records — some new NPCs
  may promote from existing survivor records), `Assets/StreamingAssets/Data/questline_master.json`
  (362 quests — NPC arcs may be quest entries with `flag_` progression)
- Check: `grep -rn "character\|Character\|npc\|Npc\|NPC" Assets/Ashfall.Core/` — does a
  system consume character data, or is this data-first?

## Content grammar (per NPC)
- snake_case `id` with prefix `npc_` (confirmed prefix in existing `characters.json`).
- name, profession, bio: grounded, human, restrained (per AGENTS.md tone).
- occupation: trader / medic / engineer / scavenger / soldier / farmer / radio_operator /
  cook / mechanic / hunter / priest / teacher / child / elder.
- skill: `skill_*` id (Plan 33) — the NPC's useful skill (a medic has `skill_field_surgery`).
- weakness: a personal flaw or vulnerability (addiction, fear, debt, loyalty, grief).
- personal_objective: what the NPC wants (find a lost relative, repay a debt, reach a
  settlement, survive long enough to see spring).
- faction_affiliation: `faction_*` id — which faction the NPC belongs to (or `independent`).
- survival_philosophy: how the NPC approaches survival (hoard, share, flee, fight, negotiate).
- trade_interest: what the NPC buys/sells (feeds Plan 13 trade flow + 16B caravans).
- secret: something the NPC hides (a crime, a betrayal, a pre-war identity, a stash).
- arc_states: list of temporal states with trigger conditions:
  - state_1 (initial): where the NPC is first encountered (Day range, location, situation).
  - state_2 (evolved): how the NPC changes (injured, promoted, disillusioned, dead) —
    triggered by a day threshold or a player action (flag).
  - state_3 (late): the NPC's final state (faction leader, corpse, ally, enemy) —
    triggered by accumulated player choices.
- possible_death: conditions under which the NPC dies (ignored when injured, killed in a
  raid, sacrificed for the group).
- possible_recruitment: conditions under which the NPC joins the shelter (rescued, convinced,
  indebted).
- later_consequence: what the NPC's arc means for the world (a promoted trader changes
  caravan routes; a dead medic leaves a settlement without medical care).

## Steps
1. Read `characters.json` to confirm the existing 36-NPC schema; the 24 new entries must
   match it. Confirm whether the schema supports arc-state fields; if not, extend
   minimally (the arc states are the core value of this plan).
2. Read `factions.json` to inventory all 19 factions; assign new NPCs across factions
   (not all in one faction — distribute for variety).
3. Read `survivors.json` to identify generic survivor records that could be promoted to
   named NPCs (a survivor with a distinctive trait becomes a named character).
4. Author 24 new NPCs across 12 occupations (2 per occupation): 2 traders, 2 medics, 2
   engineers, 2 scavengers, 2 soldiers, 2 farmers, 2 radio operators, 2 cooks, 2 mechanics,
   2 hunters, 2 priests/elders, 2 children. Each with occupation, skill, weakness,
   objective, faction, philosophy, trade interest, secret, arc states, death/recruitment
   conditions, and later consequence.
5. Design 8 temporal arcs that cross-reference existing systems:
   - Trader arc: Day 15 (trading at a waystation) → Day 40 (injured, needs medicine) →
     Day 80 (faction trade official — offers better prices if helped, embargo if refused).
   - Medic arc: Day 20 (running a makeshift clinic) → Day 50 (clinic overrun, needs
     extraction) → Day 90 (shelter medic if rescued, settlement dies if not).
   - Soldier arc: Day 25 (faction patrol) → Day 55 (deserter, hunted) → Day 100 (faction
     enemy if sheltered, ally if helped escape).
   - Child arc: Day 10 (orphaned, found at a micro-location) → Day 45 (shelter child,
     needs education) → Day 120 (apprentice, takes a skill — feeds existing 12A).
   - Engineer arc: Day 30 (stranded at a power substation) → Day 70 (shelter engineer if
     rescued, enables Plan 5 industrial recovery) → Day 150 (regional infrastructure
     project leader).
   - Priest arc: Day 35 (leading a refugee camp) → Day 75 (camp attacked, needs defense) →
     Day 130 (belief-movement leader — feeds existing 30C schism).
   - Scavenger arc: Day 18 (solo scavenger, knows a cache location) → Day 48 (injured by
     a trap, needs rescue) → Day 85 (rival scavenger guild leader — feeds existing 14).
   - Radio operator arc: Day 22 (lone operator at a radio tower) → Day 60 (tower
     discovered by a faction, needs protection) → Day 110 (faction intelligence asset —
     feeds existing 11B cipher hunts + 24A radio).
6. Cross-reference: every `skill_*` id resolves to Plan 33; every `faction_*` id resolves;
   every `loc_*` encounter location resolves to Plan 32; every `flag_*` arc trigger
   exists or is created (confirm with dialog-graph lint).
7. Wire 8 NPC arcs into `questline_master.json` as quest entries with `flag_` progression
   (encounter → evolve → resolve).
8. Wire 4 NPC arcs into Plan 50 distress signals (the medic, the engineer, the radio
   operator, the soldier — their arcs begin with a distress signal).
9. Validate: `--data-integrity-selftest`; confirm NPC arcs progress through states in a
   headless boot (advance days, confirm state transitions fire); confirm flag triggers
   work.
10. xUnit: character catalog loads, all references resolve, arc-state transitions fire on
    day/flag triggers, recruitment/death conditions apply, later consequences fire, save
    round-trip preserves NPC arc state.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
MEDIUM — the arc-state schema (step 1) is the hazard: if `characters.json` doesn't support
temporal states, extending it is a schema change (data-only if the loader is flexible, or
a minor Core change if the schema is rigid). Confirm before authoring.

## Definition of Done
- `characters.json` has 60 NPCs (36 existing + 24 new), all references resolving, 8 arcs
  wired as quest entries, 4 arcs triggered by distress signals, arc-state transitions fire
  on day/flag triggers, recruitment/death apply, later consequences fire, save round-trip
  preserves arc state, integrity + tests green.

## Follow-on
- Plan 50 (distress signals) — 4 NPC arcs begin with a distress signal.
- Plan 33 (skills) — NPC skills reference the skill catalog.
- Plan 44/45 (faction territory/patrols) — NPC faction affiliation affects patrol behavior.
- Existing 12A (generational arcs) — child NPC arcs feed the generational system.
- Existing 25A (faction life) — NPC promotions change faction leadership.
- Existing 30C (belief movements) — priest NPC arc feeds the schism content.
- W44 in roadmap 31 (recurring NPC arcs pillar).
