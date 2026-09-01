# Plan 138 — Shelter Defense & Visitor/Refugee System

## Goal

Create a shelter defense mechanic where the bunker can be attacked by raiders, faction forces, or desperate survivors, and a visitor/refugee system where outsiders arrive seeking shelter, trade, or aid. Players must decide whether to open the hatch, defend against assaults, and integrate (or expel) newcomers. This transforms the shelter from a safe box into a contested, socially dynamic space.

## Why

**Repository evidence:** `AirlockSecuritySystem.cs` (227 lines) handles airlock security but not defense against attacks. `DutyRosterPanel.cs:93` lists "Hatch Defense" as a duty role but no `ShelterDefense` system exists. `DoorEncountersSystem` (referenced in narrative data) handles visitor encounters but not shelter defense or refugee integration. No system exists for shelter attacks, siege mechanics, or refugee admission. The cross-system agent confirmed: "Shelter has NO defense mechanic and does NOT affect faction perceptions or combat."

**What is missing:** The shelter cannot be attacked. Raiders, faction patrols, or desperate survivors never assault the bunker. The hatch is an inviolate boundary. Additionally, outsiders never arrive seeking shelter — the shelter population is static after initial setup. No refugee admission, no visitor screening, no integration mechanics.

**Why existing plans don't solve it:** Plan 29 (shelter as character) covers room identity and wear but not defense. Plan 41 (shelter room catalog) adds rooms but not defense. Plan 45 (faction patrol encounters) handles external patrols but not shelter attacks. Plan 12 (social/shelter life) covers friction events but not visitors. No plan addresses shelter defense or refugee systems.

**Player value:** Creates tension (the shelter is not safe), strategic decisions (open the hatch or not?), moral choices (turn away refugees or risk resources?), and emergent stories (a refugee turns out to be a spy, a raid is repelled by clever defense).

## Files / Systems to Inspect

- `Assets/Ashfall.Core/AirlockSecuritySystem.cs` — airlock security foundation
- `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs` — duty assignment (hatch defense role)
- `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs` — combat mechanics
- `Assets/Ashfall.Core/Factions/FactionBranchCoordinator.cs` — faction relations
- `Assets/StreamingAssets/Data/door_encounters.json` — existing visitor encounters
- NEW: `Assets/Ashfall.Core/Shelter/ShelterDefenseSystem.cs`
- NEW: `Assets/Ashfall.Core/Shelter/VisitorSystem.cs`
- NEW: `Assets/StreamingAssets/Data/shelter_defense.json`
- NEW: `Assets/StreamingAssets/Data/refugee_profiles.json`

## Main Task 1 — Foundation / System Contract

1. Create `ShelterDefenseSystem.cs` in `Assets/Ashfall.Core/Shelter/`
2. Define `ShelterDefenseState` DTO: `defenseRating` (0-100), `fortificationLevel` (0-3), `activeThreats` (list of `ThreatInstance`), `lastAttackDay`, `defenseReadiness` (0-100)
3. Define `ThreatInstance` DTO: `threatType` (raiders/faction/desperate), `strength` (0-100), `approachDay`, `attackDay`, `demands` (list of item IDs or "surrender"), `factionId` (if faction attack)
4. Implement `CaptureState/RestoreState` with schema versioning
5. Define defense rating calculation: base from fortification + garrison assignment + equipment quality
6. Create fortification levels: 0 (unfortified), 1 (barricaded), 2 (reinforced), 3 (fortress)
7. Implement threat generation: random attacks based on faction standing, shelter wealth, world events
8. Create attack resolution: defense rating vs. threat strength + seeded RNG for outcomes
9. Define attack outcomes: repelled (no damage), breached (partial damage), overrun (major damage, items stolen)
10. Create `VisitorSystem.cs` in `Assets/Ashfall.Core/Shelter/`
11. Define `Visitor` DTO: `id`, `name`, `archetype` (refugee/trader/envoy/spy), `factionId`, `disposition` (friendly/neutral/hostile), `request` (shelter/trade/information/aid), `arrivalDay`, `departureDay`, `hiddenAgenda` (optional)
12. Define `VisitorState` DTO: list of current visitors, list of departed visitors, visitor log
13. Implement visitor generation: random arrivals based on world events, faction relations, shelter reputation
14. Create visitor screening: player can interrogate visitors to discover hidden agendas
15. Implement visitor outcomes: admitted (becomes temporary shelter member), turned away (leaves), detained (imprisoned), expelled (forced out)
16. Wire into `GameBootstrap`: `SetupShelterDefense`, `SetupVisitors`, `TickDefense`, `TickVisitors`, `SaveDefense`, `SaveVisitors`
17. Add deterministic seeding: threat generation and visitor arrival use `ISeededRng`
18. Create `ShelterDefenseCatalogLoader` for threat templates
19. Create `VisitorCatalogLoader` for visitor profiles

## Main Task 2 — Implementation / Defense / Visitor Mechanics

1. Implement shelter defense mechanics:
   - Fortification upgrades: spend resources to increase fortification level
   - Garrison assignment: assign survivors to hatch defense duty (reduces defense if understaffed)
   - Equipment quality: weapons/armor affect defense rating
   - Alarm system: early warning of approaching threats (gives preparation time)
   - Defensive positions: barricades, traps, chokepoints (bonus to defense rating)
2. Create attack scenarios:
   - Raider assault: 5-15 raiders, demand supplies or fight
   - Faction raid: faction forces attack (based on standing), demand surrender
   - Desperate survivors: refugees turn violent if turned away
   - Stealth infiltration: spy attempts to steal information/items
   - Siege: faction cuts off supplies, waits for surrender
3. Implement attack resolution:
   - Player chooses: defend, negotiate, surrender, flee
   - Defense: tactical combat using `TacticalCombatSystem` with shelter bonuses
   - Negotiate: skill check based on faction standing, survivor charisma
   - Surrender: lose items, faction standing penalty
   - Flee: abandon shelter temporarily, return after threat leaves
4. Create visitor arrival mechanics:
   - Random arrivals: 1-3 visitors per week (based on shelter reputation)
   - Faction envoys: diplomatic visits based on faction relations
   - Refugee groups: 2-5 refugees fleeing danger (moral choice)
   - Traders: merchants offering goods (economic opportunity)
   - Spies: hidden agenda visitors (intelligence risk)
5. Implement visitor screening:
   - Interrogation: assign survivor to question visitor (skill check)
   - Background check: research visitor's faction affiliation (cost: time)
   - Observation: monitor visitor behavior (detects hidden agendas)
   - Trust building: spend time with visitor to gain trust (reveals information)
6. Create visitor admission mechanics:
   - Temporary shelter: visitor stays 1-7 days, consumes resources
   - Permanent admission: visitor becomes shelter member (if space available)
   - Refugee integration: assign housing, work duties, monitor behavior
   - Trader access: visitor trades in designated area, no shelter access
   - Detention: visitor imprisoned (interrogation option, morale penalty)
7. Implement visitor outcomes:
   - Grateful refugee: becomes loyal survivor, morale boost
   - Hidden spy: steals information/items, escapes after 3-7 days
   - Skilled trader: offers unique goods, establishes trade route
   - Faction envoy: diplomatic mission, standing modifier
   - Desperate violent: attacks shelter if turned away (triggers defense)
8. Create visitor events:
   - "The Informant" — visitor offers intelligence on faction movements
   - "The Sick Refugee" — visitor carries disease (quarantine choice)
   - "The Skilled Worker" — visitor has valuable skill (recruitment opportunity)
   - "The Spy" — visitor is discovered stealing (confrontation choice)
   - "The Dying Stranger" — visitor needs medical aid (moral choice)
9. Add UI: "Shelter Status" panel showing defense rating, active threats, current visitors
10. Create "Visitor Log" panel showing visitor history, screening results
11. Implement defense journal: automatic log of attacks and visitor encounters
12. Create defense quest hooks:
    - "The Siege" — survive 7-day faction siege
    - "The Refugee Crisis" — manage 10+ refugees with limited resources
    - "The Spy Hunt" — identify and expel spy before they escape
    - "The Raid" — repel raider assault with minimal casualties
13. Add defense interaction with other systems:
    - `FactionBranchCoordinator`: attacks affect faction standing
    - `SurvivorRelationsSystem`: defense cooperation builds bonds
    - `MoralChoiceSystem`: refugee admission is moral choice
    - `Economy`: trader visitors affect market
14. Create 10 threat templates and 15 visitor profiles in data files

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `TacticalCombatSystem`: defense combat uses shelter bonuses
2. Connect to `FactionBranchCoordinator`: attacks/visitors affect faction standing
3. Integrate with `SurvivorRelationsSystem`: defense cooperation builds relationships
4. Connect to `MoralChoiceSystem`: refugee admission is moral choice
5. Wire into `Economy`: trader visitors affect market prices
6. Connect to `DutyRosterSystem`: garrison assignment uses duty roster
7. Implement old-save compatibility: existing saves get default defense/visitor state
8. Add deterministic seeding: threat/visitor generation uses `ISeededRng`
9. Create exploit prevention: attacks have cooldowns, visitors are time-gated
10. Add tests: defense resolution, visitor lifecycle, save round-trip, determinism
11. Verify catalog integrity: all threat/visitor IDs resolve
12. Test edge cases: no garrison (defense = 0), all visitors turned away (no integration)
13. Verify headless behavior: defense/visitor ticks correctly without UI
14. Add data-integrity-selftest: threat/visitor templates validate against catalogs
15. Create `--shelter-defense-selftest` verb for CI validation

## State / System Interaction Model

```text
Shelter defense state (fortification, garrison, equipment)
├─ Threat generated (raider/faction/desperate)
│  ├─ Approach detected (alarm system)
│  │  ├─ Player prepares: assign garrison, set traps
│  │  └─ Player unaware: surprised, defense penalty
│  ├─ Attack begins
│  │  ├─ Player chooses: defend/negotiate/surrender/flee
│  │  │  ├─ Defend: tactical combat with shelter bonuses
│  │  │  │  ├─ Repelled: no damage, morale boost
│  │  │  │  ├─ Breached: partial damage, items lost
│  │  │  │  └─ Overrun: major damage, survivors captured
│  │  │  ├─ Negotiate: skill check, faction standing
│  │  │  │  ├─ Success: threat leaves, standing modified
│  │  │  │  └─ Failure: attack proceeds
│  │  │  ├─ Surrender: lose items, standing penalty
│  │  │  └─ Flee: abandon shelter temporarily
│  │  └─ Aftermath: damage repair, survivor treatment
│  └─ Faction reaction: attack affects standing
├─ Visitor arrives (refugee/trader/envoy/spy)
│  ├─ Player chooses: admit/turn away/detain
│  │  ├─ Admit: visitor stays, consumes resources
│  │  │  ├─ Screening: interrogate/observe/research
│  │  │  │  ├─ Clean: visitor integrated
│  │  │  │  └─ Hidden agenda: spy/thief discovered
│  │  │  ├─ Integration: assign housing, work, monitor
│  │  │  │  ├─ Success: visitor becomes survivor
│  │  │  │  └─ Failure: visitor causes problems
│  │  │  └─ Departure: visitor leaves (voluntary or expelled)
│  │  ├─ Turn away: visitor leaves
│  │  │  ├─ Grateful: no consequence
│  │  │  └─ Desperate: visitor becomes threat
│  │  └─ Detain: visitor imprisoned
│  │     ├─ Interrogation: extract information
│  │     └─ Release/Expel: after investigation
│  └─ Visitor outcome affects shelter state
```

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --shelter-defense-selftest
```

## Risk

**HIGH** — Shelter defense complexity can overwhelm players if attacks are too frequent or too complex. Risk of frustration if shelter is overrun despite good defense. Mitigation: keep attacks infrequent (1-2 per month), provide clear warning, allow preparation, make defense optional (players can focus on diplomacy instead).

## Definition of Done

- `ShelterDefenseSystem.cs` exists with full `CaptureState/RestoreState`
- `VisitorSystem.cs` exists with full `CaptureState/RestoreState`
- Shelter defense mechanics functional (fortification, garrison, combat)
- Attack scenarios implemented (raiders, faction, desperate, spy, siege)
- Visitor arrival/screening/admission mechanics functional
- Visitor types implemented (refugee, trader, envoy, spy)
- Defense/visitor events and quest hooks
- Save/load round-trip tested
- Deterministic threat/visitor generation verified
- Old saves load without error
- 10 threat templates + 15 visitor profiles in data authority
- UI panels show shelter status and visitor log
- Cross-system integration (combat, factions, relations, moral choice, economy, duty roster)

## Follow-On Opportunities

- Shelter expansion (new wings for visitors/refugees)
- Shelter reputation (affects visitor quality and attack frequency)
- Shelter diplomacy (formal alliances with factions for defense)
- Shelter economy (visitors as customers, refugees as labor)
- Shelter legacy (famous defenses become shelter history)
