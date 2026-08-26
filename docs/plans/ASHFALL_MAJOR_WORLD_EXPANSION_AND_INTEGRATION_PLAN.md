# ASHFALL — Major World Expansion, Faction War, AI Combat & Living-World Integration Plan

**Document type:** implementation-grade creative + technical master plan  
**Target:** `GermanRobert-Labtester/Atomic-War-Starving-Survival`  
**Engine:** Godot 4.7+ (.NET/C#)  
**Gameplay authority:** `Assets/Ashfall.Core/`  
**Host/presentation:** `src/`  
**Data authority:** `Assets/StreamingAssets/Data/`  
**Legacy Unity tree:** `Assets/_Game/` — read-only; do not extend  
**Purpose:** convert ASHFALL from a systems-rich management game into a coherent, reactive survival world in which survivors, factions, locations, expeditions, combat, shelter systems, radio, weather, economy, quests and endings continuously influence one another.

---

# 0. Executive Direction

ASHFALL already has an unusually deep simulation foundation. The next leap should not be "add another disconnected subsystem." The correct move is to make existing systems **collide**.

The desired player experience is:

> **I make a decision in the Holdfast. That decision changes a survivor.  
> That survivor behaves differently on an expedition.  
> The expedition changes a location.  
> The location changes a faction's behavior.  
> The faction changes prices, patrols, rumors and radio traffic.  
> Those changes return to the Holdfast as consequences.  
> The game remembers all of it.**

The core development objective should therefore be a **World Consequence Spine** connecting:

`Holdfast → People → Duties → Expeditions → Encounters → Combat / Negotiation → Loot / Injury / Knowledge → Factions → World State → Radio / Rumors → Shelter Consequences → New Choices`

This document proposes:

1. A **Living Holdfast** with spatial survivor behavior.
2. A **World Fact & Consequence System** that lets systems remember and query history.
3. A **People Behavior Layer** for survivor routines, relationships, stress reactions, work, arguments, loyalty, desertion and initiative.
4. A **Faction War Runtime** supporting territorial pressure, supply lines, shelling, blockades, propaganda and dynamic alliances.
5. A **Combat AI Integration** using deterministic Utility AI, squad roles, morale and contextual behavior.
6. A **World Population Layer** for travelers, traders, patrols, refugees, deserters, scavengers, civilians, zealots and faction agents.
7. A **Quest/Storyline Network** in which location state and faction conflict alter quest structure.
8. A **Reactive Wasteland Map** with visible territorial and environmental change.
9. A **Radio / Rumor / Intelligence Network** feeding uncertainty rather than omniscient information.
10. A **Campaign Arc** spanning early survival, consolidation, faction pressure, open war and aftermath.
11. Implementation phases, data contracts, save strategy, deterministic rules, tests and acceptance criteria.

---

# 1. Non-Negotiable Engineering Rules

The creative plan must respect the project's current architecture.

## 1.1 Core / Host separation

All gameplay logic belongs in:

`Assets/Ashfall.Core/`

Godot-specific presentation belongs in:

`src/`

No new gameplay logic should be authored in the legacy Unity tree.

## 1.2 Data authority

All production content should be represented in JSON under:

`Assets/StreamingAssets/Data/`

If this document proposes a new ID, that ID is **PROPOSED ONLY** until explicitly added to the relevant JSON authority.

Do not code against a proposed ID before it exists in authoritative data.

## 1.3 Determinism

Every simulation decision that can affect state must use seeded deterministic RNG.

Never use:

- `System.Random`
- `Guid.NewGuid()`
- unordered dictionary iteration for gameplay outcomes
- machine time
- current locale

Same seed + same state must produce the same result.

## 1.4 Save integrity

Every new stateful system must implement deep-copy `CaptureState()` / `RestoreState()` and participate in checksummed persistence.

No stateful "presentation-only" cache may secretly contain game authority.

## 1.5 No runtime LLM dependency

Runtime NPC and combat behavior should use deterministic simulation / Utility AI. LLMs may assist authoring, but not moment-to-moment runtime decision making.

---

# 2. The World Consequence Spine

Create a small Core layer that becomes the connective tissue for the entire game.

## 2.1 Proposed Core module

`Assets/Ashfall.Core/Campaign/`

Recommended files:

- `WorldFact.cs`
- `WorldFactLedger.cs`
- `CampaignConsequence.cs`
- `CampaignConsequenceSystem.cs`
- `CampaignRecord.cs`
- `CampaignQuery.cs`
- `CampaignSaveState.cs`
- `ConditionExpression.cs`
- `ConditionEvaluator.cs`

## 2.2 World facts

A world fact is a normalized state statement.

Examples:

```text
location.loc_grange_hall.visited = true
location.loc_grange_hall.scavenge_count = 3
location.loc_ration_queue_plaza.state = struck
location.loc_seed_library_annex.owner = faction_rebuilders

survivor.survivor_dr_sarah_chen.trust_player = 42
survivor.survivor_gunner_mikhail.witnessed_abandonment = true
survivor.survivor_elena_vasquez.injury_leg = severe

faction.faction_rebuilders.reputation = 35
faction.faction_central_garrison.hostility = 61
faction.faction_forward_roster.toll_policy = harsh

holdfast.broadcast_beacon = true
holdfast.filter_failure_count = 2
holdfast.refugees_sheltered = 7

campaign.water_convoy_robbed = true
campaign.first_shelling_witnessed = true
campaign.war_phase = escalation
```

Do not store every transient float in the ledger. Systems remain owners of their detailed state. The fact ledger stores:

- durable decisions;
- discoveries;
- historical events;
- relationship milestones;
- quest outcomes;
- location transformations;
- faction relationship milestones;
- major resource-policy decisions.

## 2.3 Consequence records

A consequence should explain *why* something changed.

Example:

```json
{
  "id": "consequence_water_convoy_robbery",
  "day": 84,
  "source": "quest_convoy_at_the_culvert",
  "subjects": [
    "faction_rebuilders",
    "loc_culvert_road",
    "survivor_gunner_mikhail"
  ],
  "effects": [
    {"fact": "campaign.water_convoy_robbed", "value": true},
    {"fact": "faction.faction_rebuilders.reputation_delta", "value": -12},
    {"fact": "survivor.survivor_gunner_mikhail.memory", "value": "water_convoy_robbery"}
  ]
}
```

The consequence log becomes useful for:

- epilogues;
- journal auto-generation;
- survivor memory;
- faction logic;
- quest gating;
- replay summaries.

---

# 3. Living Holdfast — The Primary Play Space

The Holdfast should become the physical heart of ASHFALL.

## 3.1 Expand the visible room model

The existing five canonical Day-1 rooms should become the seed of an expandable shelter graph.

### Initial five rooms

1. Central Access Corridor
2. Air Filtration & HEPA Bay
3. Ration & Supply Locker
4. Survivor Bunk Quarters
5. Radio Tuner Station

### Later rooms / stations

These are **proposed room concepts**, not authoritative IDs yet:

- infirmary
- workshop
- decontamination bay
- hydroponics/greenhouse access
- power room
- water treatment
- quarantine room
- armory locker
- observation slit
- visitor vestibule
- archive/library
- cold storage
- waste processing
- ventilation crawlspace
- emergency escape shaft

Each room should expose:

- operational state;
- hazards;
- occupants;
- current task;
- queued maintenance;
- noise;
- radiation;
- temperature;
- air quality;
- morale atmosphere;
- structural condition.

## 3.2 Survivor spatial behavior

Survivors should no longer exist only as rows.

Create a Core behavior layer that assigns **intent**, not coordinates.

Core decides:

```text
survivor intent = rest in bunks
survivor intent = service filtration
survivor intent = speak with Elena
survivor intent = avoid Mikhail
survivor intent = eat
survivor intent = radio watch
survivor intent = treat patient
```

Godot translates intent into room anchors and movement.

### Proposed types

`Ashfall.Core.People/`

- `PersonIntent`
- `PersonActivity`
- `PersonScheduleSystem`
- `PersonBehaviorSystem`
- `PersonBehaviorContext`
- `PersonRoutineState`

## 3.3 Daily shelter rhythm

A readable day should have rhythm.

### 05:00–08:00 — Wake / ration / triage

- wake animation;
- ration distribution;
- morning arguments;
- illness symptoms;
- duty confirmation;
- radio overnight summary.

### 08:00–14:00 — Work block

- filtration;
- repair;
- greenhouse;
- medical;
- inventory sorting;
- training;
- scavenging departure.

### 14:00–18:00 — Second block

- crafting;
- decontamination;
- research;
- social encounters;
- returning expeditions.

### 18:00–22:00 — Evening pressure

- radio;
- faction news;
- visitor decisions;
- meal;
- conflict;
- planning.

### 22:00–05:00 — Night

- sleep;
- watch duty;
- insomnia;
- nightmares;
- theft;
- sabotage;
- covert meetings;
- emergency incidents.

The game does not need to animate every hour. It needs enough spatial feedback that the player can *read* what people are doing.

---

# 4. Survivor Behavior System

ASHFALL's survivors should behave as people with needs, memory and agency.

## 4.1 Four-layer behavior model

Every autonomous decision should combine:

### Layer A — Physical urgency

- thirst;
- hunger;
- fatigue;
- warmth;
- health;
- radiation sickness;
- chemical dependency;
- pain;
- contamination.

### Layer B — Assigned duty

- guard;
- filtration;
- medical;
- repair;
- cooking;
- radio;
- scavenging;
- research;
- greenhouse;
- rest.

### Layer C — Personality / relationships

- loyalty;
- fear;
- trust;
- resentment;
- compassion;
- risk tolerance;
- obedience;
- ideological stance;
- trauma triggers.

### Layer D — World context

- raid alarm;
- filter failure;
- death;
- visitor;
- faction conflict;
- food shortage;
- shelling;
- illness outbreak.

Utility AI scores candidate activities.

## 4.2 Example activity scoring

Candidate: `repair_filtration`

Positive:

- filter health low;
- survivor has mechanical skill;
- assigned filtration duty;
- high shelter loyalty.

Negative:

- severe fatigue;
- injured hand;
- fear of confined machinery room;
- urgent thirst.

Candidate: `comfort_survivor`

Positive:

- target grief high;
- friendship strong;
- current work non-critical;
- compassionate trait.

Negative:

- mutual resentment;
- active emergency;
- actor emotionally exhausted.

## 4.3 Relationship model

Create a lightweight directed relationship record:

```text
trust
affection
respect
fear
resentment
debt
dependency
```

Not every axis needs to be exposed numerically.

The player should see qualitative summaries:

- trusts;
- uneasy;
- resents;
- protective;
- afraid of;
- indebted to;
- bonded by trauma.

## 4.4 Relationship events

Examples:

- shared starvation;
- refusing medicine;
- saving someone in combat;
- leaving someone behind;
- giving someone the last clean water;
- exposing shelter coordinates by radio;
- executing / sparing a hostile;
- accepting refugees;
- refusing a family member entry;
- taking dangerous loot over medical supplies.

## 4.5 Personal memory tags

Each survivor keeps a limited memory ring.

Examples:

```text
memory_saved_by_player
memory_left_behind
memory_friend_died
memory_starved_three_days
memory_sheltered_refugees
memory_robbed_civilians
memory_shared_last_water
memory_first_kill
memory_betrayed_faction
```

Memory affects:

- Utility AI;
- dialogue;
- willingness to volunteer;
- loyalty;
- panic;
- surrender;
- desertion;
- endings.

---

# 5. People Beyond the Holdfast

The wasteland needs human presence between major quest NPCs.

Create a world-population abstraction.

## 5.1 Person categories

- independent scavenger;
- refugee;
- courier;
- caravan guard;
- faction patrol;
- deserter;
- farmer;
- medic;
- pilgrim;
- smuggler;
- scavenging family;
- child messenger;
- technician;
- wounded combatant;
- displaced civilian;
- opportunistic raider;
- recruiter;
- informant;
- spy;
- grave robber;
- salvage diver;
- water carrier.

These should not all become bespoke named NPCs.

Use archetypes + generated presentation.

## 5.2 Population record

```text
PersonRecord
- id
- archetype
- name/display label
- faction
- home location
- current location
- destination
- health band
- hunger band
- fear
- hostility
- trust
- items of significance
- memories
- rumor inventory
- current goal
```

## 5.3 World movement

People move over the location graph.

Examples:

- refugees move away from shelling;
- merchants follow safe routes;
- faction patrols concentrate at contested nodes;
- smugglers exploit blockades;
- deserters avoid faction checkpoints;
- medics migrate toward outbreaks.

The player can encounter them during expeditions.

## 5.4 Persistence tiers

Do not persist every generated traveler forever.

### Tier 1 — Named / quest-critical

Full persistence.

### Tier 2 — memorable emergent NPC

Promote to persistence after meaningful player interaction.

### Tier 3 — ambient population

Generated from deterministic seed + day + location; discarded after encounter unless promoted.

---

# 6. The Faction War System

The faction war should feel like pressure building through logistics before open violence.

## 6.1 War dimensions

Each faction tracks:

- manpower;
- food reserve;
- water access;
- ammunition;
- medicine;
- fuel;
- morale;
- territorial control;
- intelligence;
- leadership cohesion;
- public support;
- fatigue;
- war weariness.

These can be abstract scores rather than detailed inventories.

## 6.2 War actions

Factions may choose:

- patrol;
- recruit;
- fortify;
- raid;
- convoy;
- seize location;
- sabotage;
- shell;
- negotiate;
- threaten;
- blockade;
- spread propaganda;
- release prisoners;
- demand tribute;
- evacuate civilians;
- conduct false-flag action;
- send emissary;
- fracture internally.

## 6.3 Territory states

A location can be:

- neutral;
- sympathetic;
- influenced;
- occupied;
- contested;
- blockaded;
- abandoned;
- ruined.

This should affect:

- travel danger;
- encounter tables;
- trade;
- radio;
- item availability;
- civilian population;
- faction reaction.

## 6.4 Supply lines

Territory should not be a paint-the-map strategy game.

A faction should need connected supply corridors.

Example:

A Garrison checkpoint may appear strong, but if the player helps Rebuilders cut the pump route and steals the fuel convoy:

- patrol frequency decreases;
- heavy weapons disappear;
- ration complaints rise;
- deserter encounters increase;
- tolls increase;
- propaganda becomes harsher.

The world communicates logistics indirectly.

---

# 7. Major Story Campaign — "THE FRACTURE LINE"

This is a proposed multi-act campaign framework intended to integrate existing factions and expansion content.

It can sit across a broad late-midgame period without replacing existing arcs.

## Act I — Quiet Pressure

Theme: everyone is counting resources.

Events:

- patrol presence rises;
- faction representatives begin asking for lists;
- water deliveries become conditional;
- radio frequencies carry coded logistics traffic;
- survivors hear conflicting rumors;
- minor checkpoint incidents escalate.

Player question:

> "Do we stay invisible, choose partners, or make ourselves indispensable?"

## Act II — The Ledger War

Theme: bureaucracy becomes a weapon.

Factions begin controlling:

- ration cards;
- vouch lists;
- water chits;
- travel papers;
- ammunition counts;
- medicine allocations.

Quest concepts:

### "Names on the Wall"

A faction census officer requests a list of Holdfast residents.

Choices:

- give true list;
- give partial list;
- fabricate dead residents;
- refuse.

Consequences:

- future conscription;
- food access;
- suspicion;
- survivor trust.

### "Three Empty Crates"

A convoy arrives missing medicine.

Possible explanations:

- theft;
- diversion;
- corruption;
- genuine ambush.

The player investigates through radio, travelers and physical evidence.

No answer should be obvious.

## Act III — The First Shell

Theme: the world becomes physically unsafe.

A shelling hits a location the player knows.

Critical design rule:

**The player must have visited it before.**

The significance is recognition.

Map state changes.
Radio reports conflict.
Survivor memories update.

One survivor may personally know someone there.

## Act IV — The Split

The existing Forward Roster fracture becomes part of a wider systemic crisis.

Some faction members believe:

- command is too weak;
- aid is wasted on civilians;
- tolls should increase;
- the war should be forced to a conclusion.

Others fear permanent militarization.

The player can:

- supply moderates;
- expose hardliners;
- trade with both;
- secretly arm deserters;
- remain neutral;
- shelter defectors.

## Act V — Open Corridor War

Routes become dangerous.

The war is fought over:

- pumps;
- bridges;
- depots;
- tunnel mouths;
- farms;
- radio towers;
- clinics.

Not every battle is a shooting battle.

A faction can lose because:

- no antibiotics;
- frozen water;
- broken radio;
- fuel shortage;
- civilian evacuation;
- leadership split.

## Act VI — The Winter Negotiations

War exhaustion generates peace attempts.

The player can become:

- mediator;
- supplier;
- saboteur;
- profiteer;
- isolationist.

Peace is not automatically "good."

A treaty might preserve a brutal toll regime.

A military victory might end shelling but create occupation.

A failed treaty might prolong war but preserve autonomy.

---

# 8. New Faction Concepts

These are creative proposals. Each proposed faction must receive an authoritative JSON ID before any code references it.

## 8.1 The Measure Office

**PROPOSED ID:** `faction_measure_office`

A bureaucratic remnant that believes survival requires standardized accounting.

They do not primarily conquer territory.

They control:

- scales;
- ration standards;
- contamination certificates;
- weights;
- convoy manifests;
- labor records.

Tone:

calm, exact, terrifyingly procedural.

Conflict:

They can make a settlement function efficiently while gradually eliminating personal discretion.

Gameplay:

- fraud investigations;
- counterfeit chits;
- weighing disputes;
- ration standardization;
- hidden stock audits.

## 8.2 The Ash Ferrymen

**PROPOSED ID:** `faction_ash_ferrymen`

Route guides who move people through dangerous sectors.

Not heroic.

They know:

- safe culverts;
- wind shifts;
- shelter basements;
- temporary crossings.

They trade in route knowledge rather than goods.

Conflict:

Both military factions accuse them of helping enemies.

Gameplay:

- route unlocks;
- refugee movement;
- smuggling;
- evacuation;
- information economy.

## 8.3 The Quiet Ward

**PROPOSED ID:** `faction_quiet_ward`

A loose network of medical workers who refuse faction control.

They maintain hidden treatment stations.

Rules:

- no weapons inside;
- no arrests;
- patients first.

Conflict:

Every faction wants their doctors.

Gameplay:

- neutral-zone dilemmas;
- medicine shortages;
- wounded enemy care;
- outbreak response;
- moral conflicts.

## 8.4 The Cairn Cooperative

**PROPOSED ID:** `faction_cairn_cooperative`

Families maintaining food plots, burial grounds and seed stores.

They resist conscription.

Strength:

local legitimacy.

Weakness:

little military capacity.

Gameplay:

- agriculture;
- civilian survival;
- protection arrangements;
- grain politics;
- seed preservation.

---

# 9. Faction Relationship Web

Avoid simple good/evil alignment.

Example relations:

### Central Garrison ↔ Rebuilders

Mutual dependency and political hostility.

Garrison needs repair crews.
Rebuilders need corridor protection.

### Forward Roster ↔ Central Garrison

Originally internal fracture.
Roster considers command too hesitant.

### Measure Office ↔ Rebuilders

Efficient collaboration, ideological tension.

### Quiet Ward ↔ every armed faction

Medical neutrality is useful and constantly violated.

### Cairn Cooperative ↔ Rebuilders

Cooperative sympathy but fear of requisition.

### Ash Ferrymen ↔ Garrison

Accused of smuggling deserters.

The player should never be able to maximize every relationship simultaneously.

---

# 10. Faction War AI

Create deterministic faction AI that chooses strategic actions daily or weekly.

## 10.1 Proposed module

`Assets/Ashfall.Core/Factions/War/`

- `FactionWarDirector`
- `FactionWarState`
- `FactionStrategicAction`
- `FactionStrategicContext`
- `FactionTerritoryState`
- `FactionSupplyState`
- `FactionWarDoctrine`
- `FactionWarResolver`

## 10.2 Utility scores

Example action: `seize_water_plant`

Positive:

- water reserve low;
- adjacent friendly territory;
- enemy weak;
- doctrine favors infrastructure control.

Negative:

- war weariness;
- low manpower;
- recent defeat;
- player warned defenders;
- current peace talks.

## 10.3 Doctrine examples

### Attritional

Prefers blockades, raids, pressure.

### Administrative

Prefers checkpoints, documents, requisition.

### Defensive

Fortifies core territory.

### Populist

Protects civilians, propaganda, aid.

### Hardline

Shelling and reprisals.

### Opportunistic

Targets weak or isolated locations.

Doctrine influences behavior without guaranteeing it.

---

# 11. Combat AI Wiring

ASHFALL's existing tactical combat should use actual survivor/faction state.

## 11.1 Squad roles

Combatants receive a role:

- rifleman;
- support;
- medic;
- scout;
- leader;
- breacher;
- civilian;
- sniper;
- improvised fighter.

Role influences candidate actions.

## 11.2 Combat context

Core combat AI should see:

- health;
- wound severity;
- ammo;
- weapon condition;
- cover;
- suppression;
- morale;
- fatigue;
- radiation sickness;
- relationship to nearby allies;
- faction doctrine;
- mission goal;
- retreat route;
- civilian presence.

## 11.3 Enemy goals

Not every enemy wants to kill.

Goals:

- steal supplies;
- delay player;
- defend checkpoint;
- hold until reinforcements;
- capture survivor;
- break contact;
- intimidate;
- escort convoy;
- protect civilian;
- retrieve item.

Combat ends when goals are resolved, not only when one side is dead.

## 11.4 Morale

Add combat morale.

Morale falls from:

- ally down;
- leader down;
- suppression;
- low ammunition;
- severe injury;
- flanked;
- fire/explosion;
- overwhelming enemy.

Morale rises from:

- strong cover;
- leader present;
- enemy retreat;
- successful rescue;
- numerical advantage.

Possible states:

- steady;
- shaken;
- panicked;
- routing;
- surrendering.

## 11.5 Surrender

Surrender should become a narrative event.

The player may:

- disarm and release;
- take prisoner;
- exchange;
- recruit deserter;
- interrogate;
- refuse surrender.

Consequences affect faction war, survivor memory and reputation.

---

# 12. Companion Combat Behavior

Player companions should have agency.

## 12.1 Command style

Before combat, choose doctrine:

- Hold Fire
- Defensive
- Standard
- Aggressive
- Preserve Ammunition
- Protect Wounded
- Break Contact

## 12.2 Personal overrides

A survivor may disobey.

Examples:

- protects close friend;
- refuses to abandon wounded;
- flees after trauma trigger;
- uses last medical item on someone;
- shoots surrendering hostile due to hatred;
- refuses to fire on civilians.

Disobedience should be rare and explainable.

---

# 13. Expedition Preparation

Replace hard-coded demo expeditions with real loadout preparation.

## 13.1 Preparation phases

1. choose destination;
2. choose party;
3. inspect route;
4. inspect weather;
5. inspect known faction activity;
6. equip;
7. allocate supplies;
8. choose stance;
9. choose return threshold;
10. dispatch.

## 13.2 Return thresholds

Examples:

- return if any survivor health < 40%;
- return after first major encounter;
- return when carry capacity > 80%;
- return before night;
- stay until objective completed.

These become Core expedition policy.

## 13.3 Party dynamics

Survivor relationships affect expeditions.

Two survivors who hate each other:

- coordination penalty;
- argument event;
- slower treatment.

Two bonded survivors:

- rescue bonus;
- morale resilience;
- reluctance to retreat alone.

---

# 14. Location Evolution

Locations need persistence.

## 14.1 Location state

```text
visit_count
scavenge_count
loot_depletion
radiation_band
population_band
owner_faction
contested
damaged
destroyed
restored
known_routes
last_major_event_day
```

## 14.2 Depletion

Repeated scavenging should reduce common loot.

But depletion can create new opportunities.

Example:

A supermarket after repeated scavenging:

1. canned goods;
2. scrap;
3. rats;
4. hidden maintenance room;
5. cellar discovered.

The location develops rather than becoming empty.

## 14.3 Recovery

Civilian-controlled locations can recover slowly.

A farm may improve if:

- supplied water;
- protected;
- given tools;
- not requisitioned.

The player can invest in world stability.

---

# 15. World Events

Create events that cross systems.

## 15.1 Fallout front

Effects:

- expedition travel risk;
- filter wear;
- radio interference;
- crop contamination;
- faction patrol reduction.

## 15.2 Fuel collapse

Effects:

- prices;
- generator rationing;
- convoy shortages;
- faction patrols on foot;
- caravan frequency.

## 15.3 Medicine shortage

Effects:

- clinic queues;
- faction bargaining;
- theft;
- counterfeit medicine.

## 15.4 Water contamination

Effects:

- disease;
- purifier demand;
- migration;
- faction conflict around pumps.

## 15.5 Cold snap

Effects:

- heating;
- route closures;
- deaths among exposed civilians;
- ice-road opportunities.

---

# 16. Shelter Incident Framework

## 16.1 Proposed module

`Assets/Ashfall.Core/Shelter/Incidents/`

- `ShelterIncidentSystem`
- `ShelterIncidentDefinition`
- `ShelterIncidentState`
- `ShelterIncidentChoice`
- `ShelterIncidentResolver`

## 16.2 Incident examples

- filter pressure collapse;
- radon spike;
- burst pipe;
- frozen water store;
- generator stall;
- carbon monoxide warning;
- mold;
- vermin;
- food spoilage;
- electrical fire;
- structural crack;
- hatch seal damage;
- infected resident;
- missing supplies;
- visitor panic;
- internal theft;
- violent argument.

## 16.3 Labor pressure

Every emergency should compete with ongoing work.

Example:

Filter collapses.

Available survivors:

- Sarah: treating patient;
- Mikhail: repairing generator;
- Elena: expedition;
- fourth survivor: exhausted.

The player must choose what stops.

That turns system depth into tension.

---

# 17. Radio / Rumor / Intelligence Network

Information should be imperfect.

## 17.1 Information sources

- faction radio;
- civilian radio;
- travelers;
- scavenger rumor;
- returning expedition;
- intercepted coded message;
- survivor testimony;
- physical evidence.

## 17.2 Reliability

Each report has:

- source;
- age;
- reliability;
- corroboration;
- bias.

The player sees:

`LIKELY`
`UNCONFIRMED`
`CONFLICTING`
`VERIFIED`

## 17.3 Intelligence effects

Information can:

- reveal location;
- reveal patrol timing;
- reveal route hazard;
- warn of shelling;
- reveal market shortage;
- expose faction fracture.

Wrong information can create ambushes or wasted trips.

---

# 18. Quest Architecture

Move toward stateful quest graphs.

## 18.1 Quest node conditions

Support:

- day;
- world fact;
- item;
- survivor state;
- survivor relationship;
- faction relation;
- location state;
- radio knowledge;
- prior quest outcome.

## 18.2 Quest consequences

A quest should change at least two systems when significant.

Example:

Quest: protect a seed convoy.

Success:

- Cairn Cooperative relation +;
- food prices stabilize;
- new seed recipes;
- location population rises;
- one survivor memory tag;
- radio praise.

Failure:

- food prices rise;
- refugees appear;
- Rebuilders requisition food;
- future faction negotiation harder.

---

# 19. New Storyline — "THE LAST CLEAN RESERVOIR"

A midgame systemic storyline.

## Premise

A filtration survey finds that several settlements are drawing from a slowly contaminating underground reservoir.

No faction can fix it alone.

## Act 1 — Bitter Taste

Signs:

- stomach illness;
- unusual mineral deposits;
- traders refusing water.

Player can investigate.

## Act 2 — The Pump House

The treatment plant requires:

- mechanical repair;
- filter media;
- power;
- security.

Different factions offer help with conditions.

## Act 3 — The Allocation

There is not enough purified water for everyone.

Choices:

- ration equally;
- prioritize Holdfast;
- prioritize clinic;
- sell;
- give to agricultural cooperative;
- let faction authority allocate.

## Act 4 — Sabotage

A valve is damaged.

Possible causes vary by campaign state.

## Act 5 — Dry Treaty

A water compact may form.

The player can shape:

- access;
- pricing;
- armed protection;
- civilian quotas.

Long-term effects alter economy and faction war.

---

# 20. New Storyline — "THE CHILDREN OF THE VENT"

Tone: mystery without supernatural explanation.

## Premise

Children in several settlements repeat the same short rhythm tapped on pipes.

Adults dismiss it as a game.

It is actually a pattern based on old ventilation maintenance signaling.

## Development

The pattern leads to:

- sealed service tunnels;
- old shelter diagrams;
- an abandoned maintenance relay;
- hidden air-quality data;
- evidence that one district's ventilation system is failing.

Choices:

- expose information;
- sell it;
- repair system;
- evacuate residents;
- keep route secret.

This storyline connects:

- shelter;
- radiation;
- exploration;
- children/civilians;
- infrastructure;
- rumor.

---

# 21. New Storyline — "THE RED COLUMN"

## Premise

A long convoy of civilians and carts appears moving through the region.

No single faction controls it.

## Systemic function

The column physically moves between map nodes.

It consumes local resources.

It attracts:

- traders;
- disease;
- recruiters;
- thieves;
- faction agents.

Player choices:

- allow camp near Holdfast;
- trade;
- recruit survivors;
- provide medicine;
- report convoy;
- hide it.

Later the column can:

- split;
- be attacked;
- settle;
- join faction;
- disperse.

---

# 22. New Storyline — "THE UNCOUNTED"

## Premise

A faction census shows people missing from every list.

Some are:

- deserters;
- undocumented refugees;
- children;
- fugitives;
- smugglers.

The game asks:

> Is invisibility protection, or abandonment?

The player can:

- hide them;
- register them;
- create false identities;
- negotiate amnesty;
- expose a faction spy network.

Strong integration with Measure Office / census / faction war.

---

# 23. World People Behaviors

Add ambient behaviors that make settlements feel occupied.

Examples:

- queue for water;
- repair roofing;
- trade;
- argue;
- bury dead;
- carry fuel;
- escort injured;
- post notices;
- dismantle wreckage;
- patrol;
- teach children;
- cook;
- gamble;
- listen to radio;
- leave settlement.

These can be represented through:

- small sprite movement;
- ambient barks;
- event cards;
- journal summaries.

No need for full simulation of every individual.

---

# 24. Visitor System

Visitors should arrive at Holdfast physically.

## Categories

- trader;
- refugee;
- injured stranger;
- courier;
- faction representative;
- deserter;
- thief;
- old acquaintance;
- infected patient;
- lost child;
- technician.

## Decision dimensions

- allow entry;
- speak through hatch;
- search;
- quarantine;
- trade;
- refuse;
- detain.

Visitor history persists.

Someone refused today can reappear later with a faction.

---

# 25. Recruitment

Recruitment should not be a menu purchase.

Potential recruits arrive through:

- rescue;
- visitors;
- faction defections;
- caravans;
- quests.

The Holdfast needs capacity:

- food;
- water;
- bed;
- air;
- social stability.

A skilled recruit can be costly.

---

# 26. Desertion

Survivors can leave.

Drivers:

- low trust;
- repeated starvation;
- ideological conflict;
- friend death;
- forced dangerous work;
- attractive faction offer.

Desertion can be:

- announced;
- secret;
- attempted theft;
- faction defection.

The player may later encounter them.

---

# 27. Death and Aftermath

Death should echo.

Systems affected:

- relationship graph;
- duties;
- morale;
- journal;
- faction ties;
- personal questline;
- burial location.

Avoid constant melodrama.

A quiet empty bunk can be enough.

---

# 28. Economy Integration

Faction war should alter markets.

Inputs:

- road safety;
- local production;
- convoy success;
- faction blockade;
- disease;
- weather.

Effects:

- prices;
- stock;
- merchant frequency;
- counterfeit goods.

The player can manipulate market conditions by actions in the world.

---

# 29. Crafting Integration

Crafting recipes should gain world context.

Examples:

- radio repair kit after signal-station quest;
- water testing reagents after reservoir storyline;
- improvised snow sled after deep freeze;
- faction counterfeit seal after espionage quest.

Avoid recipe inflation unless each recipe connects to gameplay.

---

# 30. Research Integration

Research should respond to discoveries.

Knowledge sources:

- books;
- NPC expertise;
- disassembly;
- location inspection;
- faction cooperation;
- radio technical traffic.

Research can unlock:

- safer practices;
- diagnostics;
- efficiency;
- route prediction.

---

# 31. Disease Integration

Disease should propagate through people movement.

Sources:

- refugee column;
- contaminated water;
- crowded settlement;
- returning expedition.

Player policy:

- quarantine;
- treatment;
- refuse entry;
- distribute medicine.

Faction reaction follows.

---

# 32. Dynamic Map Presentation

The map should show:

- territory;
- known patrol risk;
- radiation;
- weather;
- refugee movement;
- caravans;
- blocked roads;
- shelling;
- active expeditions.

Use uncertain overlays when knowledge is incomplete.

---

# 33. Location Art State Variants

For important locations, support variants:

- baseline;
- damaged;
- occupied;
- abandoned;
- winter;
- post-shelling.

Do not generate variants for every location immediately.

Prioritize 10 campaign-critical locations.

---

# 34. Sound as World State

Audio should communicate simulation.

Examples:

Holdfast:
- filter hum changes with integrity;
- generator sputters;
- radio static.

Exterior:
- wind;
- distant artillery;
- convoy engines;
- bells / metal warnings.

Combat:
- suppressed panic;
- weapon condition cues.

---

# 35. UI Integration Principles

Avoid new disconnected dashboards.

Each major screen should answer:

1. What changed?
2. Why?
3. What can I do?
4. What happens if I wait?

For example, faction panel should show:

- current relationship;
- recent actions;
- current pressure;
- active demands;
- known territory.

---

# 36. Technical Data Plan

Recommended new catalogs:

```text
campaign_consequences.json
person_archetypes.json
person_behavior_actions.json
relationship_events.json
faction_war_doctrines.json
faction_war_actions.json
shelter_incidents.json
visitor_archetypes.json
rumor_templates.json
location_state_rules.json
quest_condition_definitions.json
```

All require `schema_version`.

---

# 37. Condition Grammar

Do not store free-text trigger logic.

Use typed conditions.

Example:

```json
{
  "all": [
    {"type": "day_at_least", "value": 120},
    {"type": "world_fact_equals", "key": "campaign.water_convoy_robbed", "value": true},
    {
      "any": [
        {"type": "faction_reputation_at_most", "faction_id": "faction_rebuilders", "value": -20},
        {"type": "location_state_equals", "location_id": "loc_culvert_road", "value": "occupied"}
      ]
    }
  ]
}
```

Evaluator must be deterministic and side-effect free.

---

# 38. Save Architecture

New persistent sections:

- campaign facts;
- campaign consequences;
- relationship records;
- persistent people;
- location evolution;
- faction war;
- rumors/knowledge;
- shelter incidents;
- visitor history.

Every save should:

- deep-copy;
- checksum;
- support schema migration;
- reject malformed future schema.

---

# 39. Event Bus Integration

New Core systems should publish semantic events:

```text
world_fact_changed
location_state_changed
person_intent_changed
relationship_changed
faction_action_resolved
shelter_incident_started
shelter_incident_resolved
rumor_discovered
visitor_arrived
```

Presentation listens.

Gameplay systems must not depend on UI.

---

# 40. Main.cs Decomposition

Do this incrementally.

Extract:

### `GameFlowCoordinator`

- new game;
- continue;
- game over;
- pause.

### `SaveCoordinator`

- save order;
- dirty tracking;
- flush.

### `WorldNavigationCoordinator`

- Holdfast;
- map;
- expeditions;
- location detail.

### `CampaignCoordinator`

- world facts;
- faction war;
- quests;
- world consequences.

### `OverlayRouter`

- open/close panels.

`Main.cs` remains composition root.

---

# 41. Implementation Phases

## Phase 0 — Baseline

- build green;
- tests green;
- data integrity green;
- document current authoritative IDs.

## Phase 1 — World Fact Ledger

Deliver:

- Core ledger;
- conditions;
- save;
- tests;
- simple debug panel.

Acceptance:

- facts persist;
- conditions deterministic;
- epilogue can query facts.

## Phase 2 — Living Holdfast

Deliver:

- bind all five rooms;
- survivor selection;
- room movement;
- task intents;
- room action context.

Acceptance:

- player can visually understand who is doing what.

## Phase 3 — Behavior System

Deliver:

- candidate activities;
- Utility AI;
- schedules;
- relationships;
- memory.

Acceptance:

- survivors autonomously rest, work and react.

## Phase 4 — Real Expeditions

Deliver:

- catalog-driven destinations;
- party selection;
- loadouts;
- route preparation.

Acceptance:

- no production use of hard-coded demo party.

## Phase 5 — Persistent Encounters / Combat

Deliver:

- actual party;
- real equipment/ammo;
- combat morale;
- surrender;
- consequences.

Acceptance:

- injuries and ammo persist.

## Phase 6 — Location Evolution

Deliver:

- location state;
- depletion;
- faction ownership;
- map overlays.

## Phase 7 — Population & Visitors

Deliver:

- ambient travelers;
- visitor arrivals;
- recruitment;
- desertion.

## Phase 8 — Faction War Director

Deliver:

- strategic state;
- doctrines;
- actions;
- supply lines;
- war events.

## Phase 9 — Faction War Story Integration

Deliver:

- runtime loading of existing faction-war catalogs;
- chain runner;
- location overrides;
- radio/journal integration.

## Phase 10 — Major Storyline Pack

Integrate:

- Last Clean Reservoir;
- Children of the Vent;
- Red Column;
- Uncounted.

## Phase 11 — Shelter Emergencies

Deliver:

- incident framework;
- labor conflicts;
- visible world events.

## Phase 12 — Polish

- pacing;
- UX;
- audio;
- art variants;
- performance.

---

# 42. Immediate 50-Step Action Plan

1. Freeze new expansion-system work.
2. Reconfirm current Core and Godot verification baseline.
3. Inventory canonical location IDs.
4. Inventory canonical survivor IDs.
5. Inventory faction IDs.
6. Add `Campaign` Core folder.
7. Implement `WorldFactLedger`.
8. Implement deterministic typed values.
9. Implement `ConditionExpression`.
10. Implement `ConditionEvaluator`.
11. Add save state.
12. Add checksum.
13. Add unit tests.
14. Add simple host session.
15. Add fact-debug CLI selftest.
16. Refactor HoldfastInteriorView to bind StartingLevel state.
17. Remove hardcoded room IDs.
18. Render all canonical rooms.
19. Add selected-survivor state.
20. Add room movement tween.
21. Add contextual room action panel.
22. Add PersonIntent Core model.
23. Add basic activity list.
24. Add PersonBehaviorSystem.
25. Wire hunger/thirst/fatigue urgency.
26. Wire duty assignment.
27. Add relationship records.
28. Add relationship-event application.
29. Add memory tags.
30. Persist people behavior state.
31. Replace expedition demo definitions with canonical data loader.
32. Build ExpeditionDefinition mapper from location catalogs.
33. Build party selection UI.
34. Build loadout UI.
35. Add return-policy settings.
36. Wire actual survivor IDs into combat.
37. Wire actual weapon instances into combat.
38. Remove fixed combat demo loadout from production path.
39. Add combat morale.
40. Add surrender.
41. Add location persistent state.
42. Add location state rendering.
43. Add faction war state.
44. Add faction doctrines.
45. Add faction strategic action resolver.
46. Add location-control changes.
47. Implement faction-war catalog loader.
48. Implement chain runner.
49. Add location override resolver.
50. Playtest 30 consecutive in-game days before authoring another major expansion.

---

# 43. Required Tests

## Campaign

- fact set/get;
- overwrite;
- save roundtrip;
- deterministic condition evaluation.

## People

- identical state produces identical activity selection;
- emergency overrides leisure;
- severe fatigue can override noncritical duty;
- relationship modifies social behavior.

## Expeditions

- party state persists;
- equipment consumed from real inventory;
- return outcome updates survivors.

## Combat

- AI deterministic;
- surrender possible;
- morale collapse;
- ammo conserved under doctrine.

## Faction War

- same seed same actions;
- territory changes;
- supply loss affects aggression;
- peace state suppresses hostile actions.

## Locations

- depletion;
- ownership;
- destruction;
- state survives save.

---

# 44. Headless Selftest Suggestions

Add:

```text
--campaign-facts-selftest
--people-behavior-selftest
--living-holdfast-selftest
--real-expedition-selftest
--combat-ai-selftest
--location-state-selftest
--faction-war-runtime-selftest
--visitor-system-selftest
--campaign-integration-selftest
```

The last test should simulate many days deterministically and hash key results.

---

# 45. Manual Acceptance Scenarios

## Scenario A — Broken Filter

1. Filter degrades.
2. Survivor assigned.
3. Survivor moves to filtration room.
4. Repair consumes scrap.
5. Air quality improves.
6. Journal records event.

## Scenario B — Expedition Injury

1. Real survivor dispatched.
2. Real equipment selected.
3. Encounter triggers.
4. Combat wound occurs.
5. Survivor returns injured.
6. Medical system sees injury.

## Scenario C — Faction Pressure

1. Garrison loses convoy.
2. Supply score drops.
3. Checkpoints become harsher.
4. Radio reports shortages.
5. Trade price increases.
6. Deserter encounter becomes more likely.

---

# 46. Pacing Rules

Avoid event spam.

Suggested cadence:

- minor shelter event: every 1–3 days;
- social event: 1–2/day maximum;
- serious crisis: every 4–8 days;
- major faction shift: every 7–20 days;
- campaign-changing event: rare and heavily telegraphed.

---

# 47. Narrative Tone

ASHFALL should remain:

- restrained;
- tired;
- practical;
- human;
- ambiguous.

Avoid:

- superhero NPCs;
- cartoon villains;
- magic;
- constant shocking twists;
- excessive gore;
- endless quippy dialogue.

Power comes from ordinary consequences.

---

# 48. Example Emergent Story

Day 43:

The Holdfast broadcasts a distress beacon.

Day 45:

A refugee family arrives.

The player admits them.

Day 49:

Food drops below safe reserve.

Mikhail argues the beacon was a mistake.

Day 52:

A Rebuilder convoy offers grain if one resident joins a repair crew.

The player refuses.

Day 55:

The Rebuilders remember the refusal.

Day 61:

The family’s teenage daughter recognizes a maintenance marking at a ruined pump.

That unlocks a route.

Day 64:

An expedition repairs the pump.

Day 70:

A civilian settlement gains water.

Day 82:

During faction tensions, that settlement provides the Holdfast with warning of a patrol.

None of this requires a single giant scripted quest.

It emerges from connected systems.

That should be the design target.

---

# 49. Definition of "Ready for Another Expansion"

Do not start another giant expansion until:

- Holdfast spatial gameplay works;
- real expedition parties work;
- combat uses real survivors;
- location states persist;
- factions affect routes/trade;
- world facts drive quests;
- survivor memories matter;
- visitors exist;
- faction war runtime exists;
- at least one 30-day campaign playthrough produces distinct consequences.

---

# 50. Final Priority

The highest-value order is:

1. World Fact / Consequence Spine
2. Living Holdfast
3. Survivor Behavior
4. Real Expeditions
5. Persistent Combat & Medical Consequences
6. Location Evolution
7. World Population / Visitors
8. Faction War AI
9. Faction War Story Wiring
10. New Storylines
11. Shelter Emergencies
12. Replayability
13. New expansions only after the above feels cohesive

ASHFALL has enough systems.

The next milestone is to make those systems **remember one another**.

That is what turns a feature-rich survival simulator into a world.
