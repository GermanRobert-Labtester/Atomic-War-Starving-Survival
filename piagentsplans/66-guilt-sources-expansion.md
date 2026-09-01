# Plan 66 — Guilt Sources Expansion (20 → 40 guilt triggers)

## Goal (2 lines)
Expand `guilt_sources.json` from 20 verified entries to 40. The guilt system tracks
psychological consequences of player choices (cutting rations, refusing shelter, leaving
someone behind, taking from the dead). Each guilt source has a choice pattern, severity,
description, and title. The system is wired but 20 triggers is too few for a full campaign.

## Why (P2)
- Verified: `guilt_sources.json` has 20 entries (choice_pattern, severity, description,
  title). The guilt system feeds the psychological-contamination pillar (existing 27C).
- Creates the moral-weight pillar: guilt is the invisible cost of survival decisions.
  More triggers mean more choices carry weight — the player can't avoid guilt, only
  choose which guilt to carry.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/guilt_sources.json` (expand 20 → 40 guilt triggers)
- Read-only: confirm the guilt system consumer — `grep -rn "guilt\|Guilt" Assets/Ashfall.Core/`
  to find the loader and confirm the schema (choice_pattern, severity 0.0–1.0, description,
  title)

## Content grammar (per guilt source)
- choice_pattern: the player action that triggers guilt (cut_ration, refuse_shelter,
  leave_behind, take_from_dead, abandon_quest, ignore_distress, sacrifice_survivor,
  betray_faction, execute_prisoner, steal_from_ally, etc.).
- severity: 0.1 (minor) → 1.0 (devastating) — affects how much guilt accumulates.
- description: 1-2 sentences in ASHFALL tone (cold, exhausted, human, restrained). The
  guilt is shown through physical/emotional detail, not moralizing. Skill `ashfall-write`.
- title: 2-5 words, evocative.
- system_link: optional — which system the guilt source connects to (NeedsSystem for
  ration cuts, CombatTraumaSystem for combat guilt, MemorialSystem for death guilt).

## Steps
1. Find the guilt system consumer to confirm the schema and how severity applies.
2. Read the 20 existing guilt sources to understand the choice patterns and avoid
   duplication.
3. Author 20 new guilt sources across 8 categories:
   - Resource decisions (4): hoard medicine while others die, trade away food a
     settlement needs, use contaminated supplies knowingly, burn fuel for comfort while
     others freeze.
   - Shelter decisions (3): refuse a refugee entry, expel a survivor for efficiency,
     hide a cache from allies.
   - Expedition decisions (3): abandon a quest to save resources, leave a wounded
     survivor behind, retreat from a rescue to avoid combat.
   - Combat decisions (3): execute a surrendered enemy, use civilians as bait, kill a
     former ally on the other side (feeds Plan 45/63).
   - Social decisions (3): betray a faction's trust for personal gain, inform on a
     survivor to a faction, break a promise to a dying survivor (feeds Plan 65).
   - Medical decisions (2): withhold painkillers from a dying survivor to save them,
     triage someone last because they're less useful (feeds existing 09B).
   - Scavenging decisions (1): take a family's last supplies from their home.
   - Leadership decisions (1): order a survivor to their death for the group's survival.
4. Give each source: choice_pattern, severity, description, title, optional system_link.
5. Cross-reference: every system_link references an existing system; every choice_pattern
   is unique (no duplicates).
6. Wire 5 guilt sources to Plan 57 incidents (guilt triggers fire as shelter incidents
   — a survivor confronts the player about a past choice).
7. Wire 3 guilt sources to Plan 65 final wishes (breaking a promise to a dying survivor
   generates devastating guilt).
8. Wire 3 guilt sources to existing 27C psychological contamination (accumulated guilt
   triggers trauma episodes).
9. Validate: `--data-integrity-selftest`; confirm guilt accumulates on the triggering
   choice in a headless boot.
10. xUnit: guilt catalog loads, all choice_patterns unique, severity applies correctly,
    guilt accumulates and persists, save round-trip preserves guilt state.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data + narrative authoring.

## Definition of Done
- `guilt_sources.json` has 40 guilt sources (20 existing + 20 new), all choice_patterns
  unique, 5 wired to incidents, 3 wired to final wishes, 3 wired to psychological
  contamination, guilt accumulates and persists, save round-trip green, integrity +
  tests green.

## Follow-on
- Plan 57 (incidents) — guilt triggers fire as shelter confrontations.
- Plan 65 (final wishes) — breaking a promise generates guilt.
- Existing 27C (psychological contamination) — accumulated guilt triggers trauma.
- Existing 21C (confessions) — guilt drives survivors to confess.
- Existing 30B (mourning) — guilt from death-related choices feeds mourning.
