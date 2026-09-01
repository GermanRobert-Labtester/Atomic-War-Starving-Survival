# Plan 151 — Working Animals & Companion System

## Goal

Create a working animals and companion system where survivors can tame, train, and deploy animals for shelter defense, expedition support, morale, and labor. Currently `WildlifeTrappingSystem` catches animals but they become meat/hides — no living animal companions exist. No system handles animal taming, training, deployment, or bonding. This plan adds a new gameplay layer that deepens the survival experience and creates emotional bonds with animal companions.

## Why

**Repository evidence:** `WildlifeTrappingSystem.cs` (471 lines) catches wildlife, processes meat and hides, but all animals become resources. No system exists for taming live animals, training them for tasks, or forming companion bonds. The gameplay gaps agent confirmed: "No working animals system." No class matches `animal`, `companion` (in the pet sense), `domesticat`, or `tame` as a gameplay system. `WildlifeMigrationSystem` tracks wildlife packs but they're environmental hazards, not potential companions.

**What is missing:** Players cannot befriend animals. Dogs can't guard the shelter. Cats can't boost morale. Pack animals can't carry expedition supplies. Horses can't speed travel. All wildlife is either a threat or a resource — never a partner.

**Why existing plans don't solve it:** Plan 28 (wildlife ecology) covers migration and ecology but not domestication. Plan 36 (wildlife trapping) adds traps and quarry but all catches become resources. Plan 13 (economy survival loop) mentions "active trapping/hunting" but not animal companions. No plan addresses taming, training, or deploying living animals.

**Player value:** Creates emotional bonds (animal companions), adds strategic depth (dogs for defense, horses for travel), provides morale boosts (pets reduce stress), and generates emergent stories (a loyal dog saves a survivor, a trained cat catches rats).

## Files / Systems to Inspect

- `Assets/Ashfall.Core/WildlifeTrappingSystem.cs` — wildlife catching (all become resources)
- `Assets/Ashfall.Core/WildlifeMigrationSystem.cs` — wildlife packs
- `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` — expedition logistics
- `Assets/Ashfall.Core/Shelter/` — shelter systems
- `Assets/StreamingAssets/Data/items.json` — animal items
- NEW: `Assets/Ashfall.Core/Animals/AnimalCompanionSystem.cs`
- NEW: `Assets/StreamingAssets/Data/animal_companions.json`

## Main Task 1 — Foundation / System Contract

1. Create `AnimalCompanionSystem.cs` in `Assets/Ashfall.Core/Animals/`
2. Define `AnimalCompanion` DTO: `id`, `species` (dog/cat/horse/pack_animal/bird), `name`, `ownerId` (survivor ID), `trainingLevel` (0-100), `bondStrength` (0-100), `health` (0-100), `tasks` (list of trained tasks), `age` (years), `temperament` (docile/aggressive/independent)
3. Define `AnimalCompanionState` DTO: list of animal companions, list of wild animals in shelter vicinity
4. Implement `CaptureState/RestoreState` with schema versioning
5. Define animal species with distinct capabilities:
   - **Dog**: guard duty (+20% shelter defense), expedition companion (+10% encounter detection), morale bonus (+5 to owner)
   - **Cat**: morale bonus (+10 to shelter), pest control (reduces food spoilage), independent (low maintenance)
   - **Horse**: expedition speed (+20% travel speed), cargo capacity (+50% carry weight), requires feed
   - **Pack animal** (ox/donkey): cargo capacity (+100% carry weight), slow but steady, requires feed
   - **Bird** (raven/falcon): scouting (+30% expedition visibility), message carrying, requires training
6. Define taming mechanics:
   - Wild animals can be tamed if trapped alive (cage trap)
   - Taming success based on animal temperament, survivor skill, time invested
   - Taming takes 7-30 days depending on species
   - Failed taming: animal escapes or becomes aggressive
7. Define training mechanics:
   - Trained animals gain tasks (guard, carry, scout, morale)
   - Training level increases with use (0-100)
   - Higher training = better task performance
   - Training requires time and resources (food, attention)
8. Define bonding mechanics:
   - Bond strength increases with care (feeding, grooming, play)
   - High bond = better performance, loyalty in danger
   - Low bond = animal may disobey or flee
   - Bond persists even if owner dies (animal mourns)
9. Add deterministic seeding: taming/training outcomes use `ISeededRng`
10. Wire into `GameBootstrap`: `SetupAnimalCompanions`, `TickAnimals`, `SaveAnimalCompanions`
11. Create `AnimalCompanionCatalogLoader` for species definitions
12. Implement animal health/aging: animals can get sick, age, die
13. Add animal breeding: compatible animals can produce offspring
14. Create UI hook: animal panel showing companions, tasks, bond strength

## Main Task 2 — Implementation / Taming / Training / Deployment

1. Implement taming from trapping:
   - Cage trap catch produces live animal (not meat)
   - Player assigns survivor to tame animal
   - Taming progress bar (7-30 days)
   - Success: animal becomes companion
   - Failure: animal escapes or becomes aggressive (must be put down)
2. Implement training system:
   - Assign animal to task (guard, carry, scout, morale)
   - Training level increases with daily practice
   - Higher training = better task performance
   - Multiple tasks possible but slower training
3. Implement deployment:
   - **Guard dogs**: assigned to shelter defense, detect intruders, alert to threats
   - **Expedition dogs**: accompany expeditions, detect encounters, protect owner
   - **Expedition horses**: increase travel speed, carry more supplies
   - **Pack animals**: carry heavy loads on expeditions
   - **Scout birds**: increase expedition visibility, spot dangers
   - **Morale cats**: roam shelter, provide morale bonus to all
4. Implement animal care:
   - Animals need food (species-specific diet)
   - Animals need grooming (health maintenance)
   - Animals need attention (bond maintenance)
   - Neglected animals: health drops, bond decreases, may flee
5. Implement animal combat:
   - Guard dogs defend shelter in attacks (Plan 138 integration)
   - Expedition dogs fight alongside owner in combat
   - Animals can be injured or killed in combat
   - Owner grief if animal dies (mental health impact)
6. Create animal events:
   - "The Loyal Dog" — dog saves owner from danger, bond increases
   - "The Lost Cat" — cat goes missing, search quest
   - "The Wild Horse" — rare wild horse appears, taming opportunity
   - "The Animal Doctor" — animal gets sick, veterinary care needed
   - "The Breeding" — animals produce offspring, new companion
   - "The Escape" — low-bond animal runs away
   - "The Sacrifice" — animal dies protecting owner, grief event
7. Add animal quest hooks:
   - "The Beast Tamer" — survivor with animal skill trains difficult animal
   - "The Lost Pack" — find and tame a pack of wild dogs
   - "The Race" — horse racing event for morale
   - "The Guard" — train dogs to defend against raids
   - "The Messenger" — train birds to carry messages between settlements
8. Implement animal inheritance:
   - Animal offspring inherit traits from parents
   - Trained animals can train offspring faster
   - Animal bloodlines matter (working dog lineage)
9. Add UI: animal panel showing all companions, tasks, health, bond
10. Create animal journal: automatic log of animal events
11. Implement animal tutorial: first taming explains system
12. Add animal tooltips: hover over animal shows stats and tasks
13. Create 10 animal species definitions in data file

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `WildlifeTrappingSystem`: cage trap produces live animals
2. Connect to `ExpeditionSystem`: animals accompany expeditions
3. Integrate with `ShelterDefenseSystem` (Plan 138): guard dogs defend shelter
4. Connect to `TacticalCombatSystem`: expedition dogs fight in combat
5. Wire into `NeedsSystem`: animal care consumes food resources
6. Connect to `MentalHealthCrisisSystem`: animal death causes grief
7. Implement old-save compatibility: existing saves get empty animal state
8. Add deterministic seeding: taming/training use `ISeededRng`
9. Create exploit prevention: animals have needs, can't be infinite
10. Add tests: taming, training, deployment, combat, save round-trip
11. Verify catalog integrity: all animal species IDs resolve
12. Test edge cases: no animals (no companions), many animals (resource drain)
13. Verify headless behavior: animals process correctly without UI
14. Add data-integrity-selftest: animal definitions validate against catalogs
15. Create `--animal-companions-selftest` verb for CI validation

## State / System Interaction Model

```text
Wild animal trapped alive (cage trap)
├─ Taming attempt
│  ├─ Survivor assigned to tame
│  ├─ Progress over 7-30 days
│  ├─ Success: animal becomes companion
│  └─ Failure: animal escapes/aggressive
├─ Training
│  ├─ Assign tasks (guard/carry/scout/morale)
│  ├─ Training level increases with use
│  └─ Higher training = better performance
├─ Bonding
│  ├─ Care increases bond (food, grooming, attention)
│  ├─ High bond = loyalty, better performance
│  └─ Low bond = disobedience, may flee
├─ Deployment
│  ├─ Guard dogs: shelter defense
│  ├─ Expedition animals: speed, cargo, detection
│  ├─ Scout birds: visibility, messages
│  └─ Morale cats: shelter-wide morale bonus
├─ Care
│  ├─ Food consumption
│  ├─ Health maintenance
│  └─ Neglect: health drops, bond decreases
└─ Events
   ├─ Loyal rescue, lost animal, breeding
   ├─ Illness, injury, death
   └─ Offspring with inherited traits
```

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --animal-companions-selftest
```

## Risk

**MEDIUM** — Animal system complexity can overwhelm players if too many species and tasks exist. Risk of animals feeling like just another resource to manage. Mitigation: start with 3 core species (dog, cat, horse), keep training simple, make animals emotionally engaging (names, personalities, death grief).

## Definition of Done

- `AnimalCompanionSystem.cs` exists with full `CaptureState/RestoreState`
- 5 animal species implemented (dog, cat, horse, pack animal, bird)
- Taming mechanics functional (cage trap, taming progress)
- Training system working (tasks, levels, performance)
- Bonding mechanics functional (care, loyalty, neglect)
- Animal deployment in shelter defense and expeditions
- Animal care requirements (food, health, attention)
- Animal events and quest hooks
- Save/load round-trip tested
- Deterministic taming/training verified
- Old saves load without error
- 10 animal species definitions in data authority
- UI panel shows animal companions
- Cross-system integration (trapping, expedition, shelter defense, combat, needs, mental health)

## Follow-On Opportunities

- Animal shows/competitions (morale events)
- Animal trading (buy/sell trained animals)
- Animal veterinary specialization (survivor skill)
- Animal legacy (famous animals remembered in epilogue)
- Animal mutations (radiation-exposed animals develop traits)
