# ASHFALL — Further Next Steps & Living World Milestone

**Document type:** prioritized next-step roadmap
**Project:** `GermanRobert-Labtester/Atomic-War-Starving-Survival`
**Purpose:** turn ASHFALL’s existing systems depth into a more reactive, spatial, interconnected, replayable survival world.

---

# Executive Priority

The next round of development should focus on **integration density**.

The governing principle is:

> One addition should activate several existing systems simultaneously.

ASHFALL already has substantial survival, shelter, inventory, crafting, medical, expedition, combat, faction, radio, narrative, weather, economy, research and expansion infrastructure. The strongest quality gain now comes from making those systems create consequences for one another.

The recommended direction is:

**Living Holdfast → campaign simulation → survivor behavior → real expeditions → persistent combat consequences → location evolution → visitors/population → faction-war logistics → dynamic quests → campaign chronicles.**

---

# 1. Build a 30-Day Campaign Simulation Harness

Before expanding content further, create a deterministic headless campaign runner capable of simulating:

- Days 1–30
- Days 1–90
- Days 1–180
- eventually full campaign horizons

Track:

- survivor health and needs;
- deaths;
- food and water stocks;
- shelter failures;
- expedition outcomes;
- faction reputation;
- location-state changes;
- encounters triggered;
- quests activated/completed;
- combat frequency;
- visitor frequency;
- market prices;
- major campaign facts.

Suggested command:

```bash
godot --headless --path . -- --campaign-sim 90 --seed 42069
```

Eventually add:

```bash
godot --headless --path . -- --campaign-balance-sweep 100
```

The sweep should execute many deterministic campaign seeds and summarize:

- average deaths;
- starvation frequency;
- resource collapse day;
- combat frequency;
- faction escalation timing;
- quest completion rates;
- route lock rates;
- economic inflation/deflation;
- shelter incident density.

This should become a permanent development gate.

---

# 2. Build a Campaign Director

Instead of simply adding more random events, add a deterministic director that manages **pressure distribution**.

Track campaign tension dimensions such as:

```text
scarcity_pressure
shelter_pressure
social_pressure
faction_pressure
radiation_pressure
medical_pressure
exploration_pressure
war_pressure
```

The director should not create resources or cheat outcomes. It only modifies the weighting of already-eligible events.

---

# 3. Add Delayed Consequences

Not every choice should resolve immediately.

Example:

- Day 12: player gives medicine to refugees.
- Day 24: a traveler recognizes the Holdfast.
- Day 51: their settlement sends intelligence.
- Day 83: a faction demands an explanation.

Create a Core system such as:

```text
DelayedConsequenceSystem
```

Support:

- execute on day;
- execute after N days;
- execute when condition becomes true;
- expire after a date;
- cancel when contradictory state occurs.

---

# 4. Build a World Rumor Generator

Use authored, deterministic templates rather than runtime generative AI.

Example input:

```text
subject = Rebuilders
action = seized
location = pump station
reliability = 0.61
source = caravan
```

Rumors should evolve through:

```text
unheard
→ rumored
→ corroborated
→ verified
→ disproven
```

Sources may include faction radio, civilian radio, returning expeditions, traders, refugees, caravans, intercepted traffic, survivor testimony and physical evidence.

---

# 5. Add Location Secrets

Important locations should contain more than loot.

Give locations discovery states such as:

```text
unknown
surveyed
partially_understood
fully_understood
```

Possible secrets:

- hidden room;
- safe route;
- buried cache;
- faction connection;
- former resident;
- contamination source;
- alternate entrance;
- infrastructure link;
- concealed survivor;
- old relay equipment;
- pre-war records.

---

# 6. Add Multi-Visit Location Storytelling

Do not exhaust important locations after one expedition.

Example progression:

1. Abandoned greenhouse.
2. A door has recently been repaired.
3. A family is living there.
4. A faction representative arrives.
5. The family asks the Holdfast to choose sides.

Do this for 8–12 flagship locations before adding hundreds of new ones.

---

# 7. Build Expedition Objectives

Examples:

- scavenge;
- rescue;
- survey;
- sabotage;
- escort;
- deliver;
- retrieve;
- investigate;
- negotiate;
- establish contact;
- repair infrastructure;
- evacuate civilians;
- recover a specific item;
- map a route.

Objective should modify party recommendations, loadouts, event tables, victory conditions and return logic.

---

# 8. Add Expedition Roles

Suggested roles:

```text
leader
scout
porter
medic
technical specialist
security
negotiator
```

A survivor may fill several roles poorly or one role exceptionally.

---

# 9. Add Party Cohesion

Compute expedition cohesion from relationships, leadership, fatigue, trauma, ideology and previous shared experiences.

Low cohesion can cause arguments, hesitation, disobedience and poor retreat coordination.

High cohesion can improve rescue, morale resilience, supply sharing and coordinated withdrawal.

---

# 10. Build Noncombat Resolution AI

Combat should be one branch of encounter resolution.

NPC groups need deterministic Utility-AI choices for:

- negotiate;
- flee;
- bluff;
- intimidate;
- trade;
- hide;
- surrender;
- demand toll;
- ask for help;
- betray;
- stall;
- call reinforcements.

---

# 11. Give Factions Tactical Personalities

## Central Garrison

- demands documentation;
- prefers capture;
- disciplined withdrawal;
- conserves ammunition.

## Forward Roster

- aggressive toll enforcement;
- intimidation;
- lower willingness to retreat;
- more volatile morale.

## Rebuilders

- defend technical infrastructure;
- favor negotiation around machinery;
- prioritize engineers.

---

# 12. Add Civilian Presence to Combat Areas

Possible complications:

- civilians hiding nearby;
- wounded neutral;
- trapped family;
- merchant caravan;
- medical station;
- refugees crossing;
- workers repairing infrastructure.

Combat objectives can therefore become extraction, defense, rescue or de-escalation rather than simple elimination.

---

# 13. Add Combat Noise Consequences

Every fight should generate a noise signature.

Noise may:

- attract hostile groups;
- reveal expedition position;
- frighten civilians;
- increase faction awareness;
- alter encounter probability;
- trigger patrol response;
- damage local reputation.

---

# 14. Add Post-Combat Decisions

Possible options:

- take prisoners;
- release wounded enemies;
- confiscate equipment;
- provide treatment;
- exchange captives;
- search the area;
- destroy evidence;
- withdraw immediately.

Each choice can affect faction relations, survivor memory, future encounters, rumors, quest state and campaign records.

---

# 15. Add Wounded / Missing / Captured States

Add:

```text
missing
captured
stranded
wounded
lost_contact
```

These states naturally generate rescue stories.

---

# 16. Build Emergent Quest Generation from State

Assemble quests from authored templates and real game state.

Examples:

```text
survivor captured
→ Recover [Survivor]
```

```text
damaged water location
→ Repair the Pump
```

```text
missing caravan
→ Find the Convoy
```

```text
infected settlement
→ Secure Antibiotics
```

---

# 17. Add Survivor Personal Goals

Possible goals:

- find missing sibling;
- maintain clinic;
- avoid military faction;
- preserve technical books;
- build radio;
- reach coast;
- recover keepsake;
- protect another survivor;
- settle a personal debt.

---

# 18. Add Survivor Secrets

Possible secrets:

- former faction membership;
- concealed illness;
- family connection;
- theft;
- false identity;
- hidden technical expertise;
- previous collaboration with hostile faction.

---

# 19. Add Ideological Conflict Inside the Holdfast

Survivors should develop opinions on:

- refugees;
- faction cooperation;
- ration equality;
- weapon trade;
- enemy medical treatment;
- radio broadcasting;
- conscription;
- neutrality.

Repeated choices can create informal blocs and interpersonal pressure.

---

# 20. Add Leadership / Legitimacy

Track dimensions such as:

```text
competence
fairness
fear
trust
dependency
```

Leadership should affect obedience during crises.

---

# 21. Create Shelter Social Spaces

## Bunks
Private conversations, grief, insomnia, relationships.

## Radio Room
Information disputes, secret listening, faction contact.

## Storage
Theft, suspicion, ration conflict.

## Food Area
Arguments over portions and morale.

## Infirmary
Confessions, fear and personal revelations.

---

# 22. Add Infrastructure Chains

Example:

```text
power
→ filtration
→ air quality
→ health
→ worker availability
→ repair capacity
```

Another:

```text
water
→ hygiene
→ disease
→ medical consumption
→ trade demand
```

---

# 23. Build Redundancy Systems

Examples:

- secondary filter;
- backup water tank;
- emergency battery;
- redundant radio;
- alternate exit;
- reserve pump;
- secondary heater.

Late-game progress should increasingly mean resilience, not only bigger numbers.

---

# 24. Add Failure Cascades

Example:

```text
generator failure
→ ventilation weakens
→ radon rises
→ technician becomes sick
→ repair capacity drops
→ expedition delayed
```

Use the Campaign Director to prevent cascades from becoming constant punishment.

---

# 25. Build Faction Logistics Convoys

Convoys should exist as moving world entities.

Player options:

- observe;
- trade;
- escort;
- raid;
- warn;
- sabotage;
- ignore.

Possible convoy types:

- food;
- water;
- medicine;
- ammunition;
- fuel;
- recruits;
- prisoners;
- engineering equipment.

Their success changes faction state.

---

# 26. Add Territory Intelligence Uncertainty

Examples:

```text
Rebuilders — likely control
Garrison — last verified 6 days ago
Contested — unconfirmed
```

Information may come from scouts, radio, travelers, intercepted reports or direct expedition contact.

---

# 27. Add Faction Internal Politics

Each major faction should contain internal tendencies.

Example Central Garrison:

- professional officers;
- logistics administration;
- hardliners;
- exhausted conscripts.

Player actions influence which tendency gains influence.

---

# 28. Add Concrete Peace-Failure Reasons

Examples:

- water access;
- prisoner exchange;
- territorial corridor;
- leadership assassination;
- food shortage;
- internal hardliners;
- checkpoint authority;
- disputed convoy.

The player can intervene in actual causes rather than arbitrary scripted failure.

---

# 29. Build Endings from Campaign Records

Use:

- survivors alive/dead;
- shelter condition;
- faction balance;
- relationships;
- infrastructure;
- reputation;
- location states;
- major world facts.

The ending should reflect what the campaign became because of repeated player behavior.

---

# 30. Add Campaign Chronicles

Generate a campaign chronology from consequence records.

Example:

```text
Day 4 — The Holdfast acknowledged the coastal frequency.
Day 17 — The filtration stack failed during black rain.
Day 31 — Sarah Chen returned from the Grange with radiation burns.
Day 46 — The Holdfast refused the Garrison census.
Day 79 — The ration plaza was shelled.
```

Use the same record for save summaries, epilogues, player history and campaign retrospectives.

---

# ASHFALL — THE LIVING WORLD MILESTONE

This should be the next major quality milestone.

## Definition of Done

- five-room Holdfast fully spatial;
- survivor autonomous behavior;
- relationships and memories;
- World Fact / Consequence Ledger;
- canonical expedition destinations;
- real party and loadout selection;
- persistent combat consequences;
- wounded/missing/captured states;
- dynamic location states;
- visitors and travelers;
- faction-war AI;
- moving logistics convoys;
- radio and rumor uncertainty;
- at least one multi-visit location storyline;
- at least one emergent rescue quest;
- at least one faction conflict visibly changing prices or routes;
- deterministic 30-day simulation;
- one manually played 30-day campaign producing a coherent narrative without developer intervention.

---

# Recommended Development Order

## Stage 1 — Simulation Observability

1. Campaign simulation harness.
2. Campaign balance sweep.
3. World Fact Ledger.
4. Consequence records.
5. Delayed consequences.
6. Campaign chronicle.

## Stage 2 — Living Holdfast

7. Complete spatial room binding.
8. Survivor selection and movement.
9. Activity intents.
10. Shelter social spaces.
11. Relationship state.
12. Survivor goals and memories.
13. Leadership state.
14. Shelter emergency interactions.

## Stage 3 — Real Expeditions

15. Remove production dependence on demo destinations.
16. Canonical location mapping.
17. Expedition objectives.
18. Party roles.
19. Real loadouts.
20. Return thresholds.
21. Party cohesion.
22. Missing/captured states.

## Stage 4 — Encounter & Combat Integration

23. Noncombat Utility AI.
24. Faction tactical behavior.
25. Real party combat.
26. Combat morale.
27. Surrender.
28. Civilian presence.
29. Noise consequences.
30. Post-combat resolution.

## Stage 5 — Living Wasteland

31. Location secrets.
32. Multi-visit locations.
33. Persistent location states.
34. Territory uncertainty.
35. Moving visitors/travelers.
36. Convoys.
37. World rumor network.

## Stage 6 — Faction War Depth

38. Faction logistics.
39. Faction internal politics.
40. Strategic doctrines.
41. Territory influence.
42. Supply disruptions.
43. Concrete peace conditions.
44. Open-war escalation.

## Stage 7 — Dynamic Storytelling

45. Emergent quest assembly.
46. Personal survivor arcs.
47. Rescue missions.
48. Infrastructure-driven quests.
49. Campaign-record-driven endings.
50. Full 30-day manual acceptance playthrough.

---

# Verification Gates

Recommended new headless tests:

```text
--campaign-sim-selftest
--campaign-balance-selftest
--world-facts-selftest
--delayed-consequence-selftest
--people-behavior-selftest
--relationships-selftest
--living-holdfast-selftest
--real-expedition-selftest
--party-cohesion-selftest
--combat-ai-selftest
--combat-morale-selftest
--location-evolution-selftest
--world-population-selftest
--faction-logistics-selftest
--rumor-network-selftest
--campaign-chronicle-selftest
--living-world-integration-selftest
```

The final integration test should simulate a deterministic campaign and hash major outcomes.

---

# Final Priority Summary

The highest-return sequence is:

1. Campaign simulation harness
2. World Fact / Consequence Ledger
3. Living Holdfast
4. Survivor behavior and relationships
5. Real expeditions and party roles
6. Persistent field consequences
7. Combat AI + morale + surrender
8. Location evolution
9. Visitors and ambient population
10. Rumor and intelligence network
11. Faction logistics and internal politics
12. Emergent quests
13. Campaign chronicles
14. Campaign-record-driven endings

ASHFALL's next leap should come from making the game **remember, react and propagate consequences**, rather than from adding another large disconnected feature set.
