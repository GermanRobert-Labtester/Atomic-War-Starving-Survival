# Plan 63 — Warlord Doctrines Expansion (12 → 24 doctrines)

## Goal (2 lines)
Expand `warlord_doctrines.json` from 12 verified entries to 24. Warlord doctrines define
the behavior of warlord factions (existing 10A bestiary + 25C war escalation) — each
doctrine is a tactical/strategic profile that affects how a warlord faction patrols,
raids, recruits, and escalates. The system is wired but 12 doctrines is too few for
variety across 19 factions.

## Why (P2)
- Verified: `warlord_doctrines.json` has 12 entries; the warlord system is part of the
  faction-war arc (existing 06C/25C). Doctrines determine how warlord factions behave in
  the faction-territory-patrol loop (Plans 44/45).
- Creates the warlord-variety pillar: each warlord faction has a distinct doctrine that
  makes it behave differently on the map — some raid aggressively, some fortify, some
  recruit, some infiltrate. Without variety, all warlords feel the same.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/warlord_doctrines.json` (expand 12 → 24 doctrines)
- Read-only: `Assets/StreamingAssets/Data/factions.json` (19 factions — doctrines may
  reference `faction_*` ids), `Assets/StreamingAssets/Data/faction_war_events.json`
  (22 chains — warlord doctrines affect which event chains fire)

## Content grammar (per doctrine)
- snake_case `id` with prefix `doctrine_` (confirm accepted prefix from existing 12).
- doctrine_type: raider / fortifier / recruiter / infiltrator / extortionist / warlord /
  prophet_warlord / technologist / slaver / warlord_council.
- patrol_behavior: aggressive (seeks combat) / defensive (holds territory) / opportunistic
  (attacks weak targets) / evasive (avoids superior forces).
- raid_frequency: how often this doctrine generates raids (feeds existing 14).
- recruitment_method: coercion / ideology / payment / kidnapping (feeds Plan 45 press gangs).
- escalation_trigger: what causes this doctrine to escalate to faction war (territory
  loss, leader death, resource shortage, player provocation — feeds existing 06C).
- preferred_targets: settlement types, caravan routes, or faction territories this
  doctrine prefers to attack.
- weakness: what this doctrine is vulnerable to (cut supply lines, kill the leader,
  destroy the recruitment base — gives the player a counter-strategy).
- faction_link: optional `faction_*` id — which faction uses this doctrine.

## Steps
1. Read `warlord_doctrines.json` to confirm the 12 existing doctrines and their schema.
2. Read `faction_war_events.json` to confirm how doctrines affect event-chain selection.
3. Read `factions.json` to identify which factions are warlord-type (not all 19 are
  warlords — classify which need doctrines).
4. Author 12 new doctrines across 10 types:
   - 2 raiders (fast, aggressive, hit-and-run — high raid frequency, low defense).
   - 2 fortifiers (dig in, defend territory — low raid frequency, high defense).
   - 1 recruiter (focus on building numbers — press gang patrols, Plan 45).
   - 1 infiltrator (spy, sabotage, false flags — feeds Plan 50 false-flag signals).
   - 1 extortionist (taxes routes, demands tribute — feeds Plan 40 debt).
   - 1 prophet_warlord (ideological warfare — feeds existing 30C belief movements).
   - 1 technologist (uses salvaged tech, drones, automated turrets — feeds Plan 54).
   - 1 slaver (captures survivors for labor — feeds Plan 52 NPC arcs).
   - 1 warlord_council (multi-leader, internal politics — feeds existing 25A faction life).
5. Give each doctrine: patrol behavior, raid frequency, recruitment method, escalation
   trigger, preferred targets, weakness, faction link.
6. Cross-reference: every `faction_*` link resolves; every escalation trigger references
   valid event chains or flags.
7. Wire 6 doctrines into Plan 45 patrol encounters — each doctrine generates different
   patrol types (raider = raid party, fortifier = border patrol, recruiter = press gang).
8. Wire 4 doctrines into existing 14 raid encounters — raid frequency determines how
   often raids fire.
9. Wire 3 doctrines into existing 06C faction war escalation — escalation triggers
   determine when a warlord faction goes to war.
10. Validate: `--data-integrity-selftest`; confirm doctrines affect patrol/raid/war
    behavior in a headless boot.
11. xUnit: doctrine catalog loads, all references resolve, patrol behavior affects
    encounter type, raid frequency is deterministic (seeded), escalation triggers fire
    on correct conditions, save round-trip preserves doctrine state.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data.

## Definition of Done
- `warlord_doctrines.json` has 24 doctrines (12 existing + 12 new), all references
  resolving, 6 wired to patrols, 4 wired to raids, 3 wired to war escalation, patrol/
  raid/war behavior varies by doctrine, save round-trip green, integrity + tests green.

## Follow-on
- Plan 45 (patrols) — doctrines determine patrol type and behavior.
- Existing 14 (raids) — raid frequency per doctrine.
- Existing 06C (faction war) — escalation triggers per doctrine.
- Plan 54 (combat catalog) — technologist doctrine uses automated turrets and drones.
- Plan 52 (NPC arcs) — slaver doctrine captures NPCs for labor.
- Existing 30C (belief movements) — prophet warlord doctrine feeds the schism content.
