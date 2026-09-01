# Wave 17 — Summary (Plans 146–150)

## Wave Overview

Five non-duplicative, implementation-ready plans covering cross-system bridges, social depth, narrative generation, infrastructure, and family dynamics. Each plan addresses a verified gap confirmed by repository inspection and background agent analysis.

| Plan | New Capability | Why It Is Not Duplicate | Risk | Key Systems |
| ---- | -------------- | ----------------------- | ---- | ----------- |
| 146 — Radiation → Economy & Social Bridge | Contaminated goods trade restrictions, discrimination against irradiated survivors, faction reactions to radiation. | Plan 137 needs→performance doesn't address radiation. Plan 106 dose items adds content but not integration. Verified: zero economy/social references to radiation state. | MEDIUM | RadiationSystem, MarketSystem, FactionStanceEngine, SurvivorRelations, DecontaminationSystem |
| 147 — Per-NPC Memory & Relationship Depth | Individual NPCs remember specific player actions and change behavior. Not just faction-level trust. | Plan 52 recurring NPC arcs adds NPCs but not memory. Plan 132 hidden agendas adds secrets but not NPC memory. Verified: no per-NPC memory system exists; all "memory" is faction-level or flag-level. | MEDIUM | HoldfastNpcCatalog, FactionStanceEngine, VerdictNpcSystem, DoorEncounterSystem, HoldfastTradeSession |
| 148 — Ideological Friction → Events & Quests | Friction produces confrontation events, conversion attempts, bunker splits, and quest chains. Not just sleep penalties. | Plan 12 social/shelter life mentions friction but doesn't detail events. Plan 144 autonomy adds behavior but not ideology-specific. Verified: `OnFrictionDetected` event has zero subscribers; friction produces only numerical modifiers. | MEDIUM | IdeologicalFrictionSystem, SurvivorRelations, MoralChoice, DutyRoster, MentalHealthCrisis |
| 149 — Persistent Achievement & Milestone System | Cross-campaign achievement tracking with rewards and meta-progression. | Plan 140 legacy adds inheritance but not achievement tracking. Verified: `AchievementsPanel.cs` derives 6 milestones from live state with comment "An AchievementsHostSession does not exist yet"; no persistence, no rewards, no data file. | LOW | CampaignCalendar, SurvivorFateSystem, ExpeditionSystem, FactionBranchCoordinator, MoralChoiceSystem, MarketSystem |
| 150 — Romance & Family Dynamics System | Romantic relationships, courtship, partnership, family units, parent-child dynamics, generational storytelling. | Plan 12 mentions generational arcs but not romance. Plan 30 covers rituals but not family. Plan 144 autonomy adds behavior but not romance. Verified: no romance system, no family units, `bondType` field never set to romantic/family types. | MEDIUM | SurvivorRelations, CohortSystem, GenerationalLineageExtension, CaregivingSystem, MentalHealthCrisis, DutyRoster |

## Strongest Plan to Implement First

**Plan 146 — Radiation → Economy & Social Bridge.** It fixes a verified gap (radiation has zero economic/social impact), has clear scope (connect existing radiation system to economy/social systems), moderate risk (straightforward modifier system), and immediate player value (radiation management becomes strategically meaningful beyond health). It also creates foundation for Plan 147 (NPCs react to irradiated survivors).

## Dependencies Between the 5 Plans

- **Plan 146 (Radiation Bridge) is standalone** but creates social context for Plan 147 (NPCs remember irradiated survivors).
- **Plan 147 (Per-NPC Memory) is standalone** but benefits from radiation bridge (NPCs remember radiation-related interactions).
- **Plan 148 (Friction Events) is standalone** but friction events can reference NPC relationships (147) and family dynamics (150).
- **Plan 149 (Achievements) is standalone** but can track romance/family milestones (150) and friction resolution (148).
- **Plan 150 (Romance/Family) is standalone** but family events can trigger friction (148) and create achievement opportunities (149).

## Recommended Implementation Order

1. **Plan 146** — Radiation → Economy & Social Bridge (cross-system bridge, standalone)
2. **Plan 147** — Per-NPC Memory & Relationship Depth (social depth, builds on 146)
3. **Plan 148** — Ideological Friction → Events & Quests (narrative generation, uses 147)
4. **Plan 150** — Romance & Family Dynamics System (social/family depth, uses 147/148)
5. **Plan 149** — Persistent Achievement & Milestone System (infrastructure, tracks all others)

## Why This Wave Materially Expands ASHFALL

These five plans transform ASHFALL from a game with isolated systems into one where radiation has social weight (contaminated goods, discrimination), NPCs remember you personally (not just faction-level), beliefs create drama (confrontations, conversions, splits), accomplishments persist across campaigns (achievements with rewards), and survivors form families (romance, children, generational stories). Each plan closes a verified gap — bridges built, memory added, events generated, achievements tracked, families formed — creating a game where every interaction matters, every choice is remembered, and every survivor has a story worth telling.
