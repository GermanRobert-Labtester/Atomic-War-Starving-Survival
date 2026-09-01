# Plan 174 — Procedural Survivor Backstories & Origin Mechanics

## Goal

Create a procedural survivor backstory system where each survivor arrives with a mechanically-relevant personal history — previous occupation, life experiences, relationships, traumas, and skills — that shapes their behavior, capabilities, and interactions. Currently `YearOfAshCatalogLoader.cs` has a single `backstory` string field for narrative flavor, but backstories have no mechanical effect. Survivors arrive as blank slates with no personal history influencing their gameplay. This plan adds biographical depth that makes every survivor unique and mechanically distinct.

## Why

**Repository evidence:** Grep for `backstory`, `origin_story`, `personal_history`, `survivor_background`, `BackstorySystem` in Core returns 1 match: `YearOfAshCatalogLoader.cs:66` — `public string backstory = string.Empty;` — a single string field for narrative flavor only. No backstory system, no mechanical backstory effects, no procedural backstory generation. Survivors have traits and skills but no personal history that explains why they have those traits or how their past shapes their present.

**What is missing:** No mechanically-relevant backstories. No procedural backstory generation. No backstory-driven behavior. No backstory-driven interactions. Every survivor is a collection of stats and traits with no personal narrative. No explanation for why a survivor has medical training or combat experience. No backstory-based quest hooks. No backstory-influenced relationships.

**Why existing plans don't solve it:** Plan 144 (survivor autonomy) adds autonomous behavior but not biographical depth. Plan 147 (per-NPC memory) adds memory of events but not pre-existing personal history. Plan 150 (romance/family) adds relationship mechanics but not backstory-driven attraction. Plan 154 (education) adds skill learning but not pre-existing knowledge from past life. No plan addresses procedural survivor backstories with mechanical effects.

**Player value:** Creates unique survivors (every survivor has a different story), adds roleplay depth (survivors have histories), generates emergent narratives (backstory-driven interactions), makes recruitment meaningful (each new arrival is a story), and connects survivor mechanics to narrative.

## Files / Systems to Inspect

- `Assets/Ashfall.Core/Survivors/` — survivor systems
- `Assets/Ashfall.Core/Survivors/SurvivorLifecycle.cs` — survivor creation
- `Assets/Ashfall.Core/Survivors/SurvivorRelationsSystem.cs` — relationships
- `Assets/Ashfall.Core/Survivors/SkillProgressionSystem.cs` — skills
- `Assets/StreamingAssets/Data/survivors.json` — survivor definitions
- NEW: `Assets/Ashfall.Core/Survivors/BackstorySystem.cs`
- NEW: `Assets/StreamingAssets/Data/backstory_templates.json`

## Main Task 1 — Foundation / System Contract

1. Create `BackstorySystem.cs` in `Assets/Ashfall.Core/Survivors/`
2. Define `BackstoryTemplate` DTO: `templateId`, `occupation` (doctor/soldier/farmer/engineer/teacher/merchant/pilot/artist/cleric/mechanic), `lifeExperiences` (list of experience IDs), `startingSkills` (list of skill bonuses), `startingTraits` (list of trait modifiers), `relationships` (list of pre-existing relationship hooks), `traumas` (list of trauma types), `possessions` (list of starting items), `flavorTexts` (list of narrative descriptions)
3. Define `SurvivorBackstory` DTO: `survivorId`, `templateId`, `occupation`, `lifeExperiences` (list), `preWarLife` (description), `definingMoment` (key life event), `lostConnections` (list of people left behind), `reasonForSurvival` (motivation), `secrets` (list of hidden backstory elements)
4. Define `LifeExperience` DTO: `experienceId`, `experienceName` (combat/medical/leadership/survival/technical/social/creative/athletic/intellectual/spiritual), `effect` (skill bonus, trait modifier, behavior modifier), `rarity` (common/uncommon/rare)
5. Define `BackstoryState` DTO: list of survivor backstories, list of available templates, backstory generation seed, revealed secrets (list of survivorId + secretId pairs)
6. Implement `CaptureState/RestoreState` with schema versioning
7. Define survivor occupations:
   - **Doctor**: +medical skill, +empathy trait, -combat skill, starts with medical kit
   - **Soldier**: +combat skill, +discipline trait, -social skill, starts with weapon
   - **Farmer**: +agriculture skill, +patience trait, -technical skill, starts with seeds
   - **Engineer**: +technical skill, +problem-solving trait, -social skill, starts with tools
   - **Teacher**: +education skill, +patience trait, -combat skill, starts with books
   - **Merchant**: +trade skill, +charisma trait, -combat skill, starts with trade goods
   - **Pilot**: +navigation skill, +courage trait, -medical skill, starts with map
   - **Artist**: +creative skill, +sensitivity trait, -combat skill, starts with art supplies
   - **Cleric**: +spiritual skill, +empathy trait, -combat skill, starts with religious item
   - **Mechanic**: +technical skill, +practical trait, -social skill, starts with wrench
8. Define life experiences:
   - **Combat**: military service, self-defense, violence survived → +combat skills
   - **Medical**: healthcare worker, first aid training, plague survivor → +medical skills
   - **Leadership**: managed team, elected official, community organizer → +leadership
   - **Survival**: wilderness experience, disaster survivor, refugee → +survival skills
   - **Technical**: engineering, programming, mechanics → +technical skills
   - **Social**: counseling, teaching, sales → +social skills
   - **Creative**: art, music, writing → +creative skills
   - **Athletic**: sports, military training, manual labor → +physical skills
   - **Intellectual**: research, academia, analysis → +intellectual skills
   - **Spiritual**: religious leadership, meditation, philosophy → +spiritual skills
9. Define backstory generation:
   - Each new survivor gets procedurally generated backstory
   - Generation selects occupation (weighted by rarity)
   - Generation selects 2-4 life experiences
   - Generation creates defining moment
   - Generation creates lost connections
   - Generation creates reason for survival
   - Generation creates 0-2 secrets
   - All generation uses `ISeededRng` for determinism
10. Define mechanical effects:
    - Occupation determines starting skill bonuses
    - Life experiences add skill bonuses and trait modifiers
    - Defining moment affects personality and behavior
    - Lost connections create relationship hooks (may arrive as NPCs)
    - Reason for survival affects morale and motivation
    - Secrets create reveal events (backstory disclosure)
11. Define backstory revelation:
    - Some backstory elements hidden initially
    - Revealed through interaction (conversation, events, time)
    - Secret revelations create events and quest hooks
    - Trust affects revelation speed (high trust = faster revelation)
    - Some secrets are negative (criminal past, betrayal)
    - Some secrets are positive (hidden skills, valuable connections)
12. Add deterministic seeding: backstory generation uses `ISeededRng`
13. Wire into `GameBootstrap`: `SetupBackstories`, `TickBackstories`, `SaveBackstories`
14. Create `BackstoryTemplateCatalogLoader` for template definitions
15. Implement backstory UI: survivor detail panel showing backstory

## Main Task 2 — Implementation / Generation / Effects / Revelation / UI

1. Implement backstory generation:
   - New survivor created → backstory generated
   - Occupation selected (weighted by game state needs)
   - Life experiences selected (2-4, weighted by occupation)
   - Defining moment generated
   - Lost connections generated (0-3 people)
   - Reason for survival generated
   - Secrets generated (0-2)
   - Backstory stored with survivor
2. Implement starting bonuses:
   - Occupation grants skill bonuses
   - Life experiences grant additional bonuses
   - Starting possessions granted
   - Trait modifiers applied
   - Bonuses visible in survivor detail
3. Implement backstory-driven behavior:
   - Occupation affects work assignment preference
   - Life experiences affect skill checks
   - Defining moment affects stress response
   - Reason for survival affects morale modifiers
   - Lost connections affect relationship priorities
4. Implement backstory revelation:
   - Hidden backstory elements revealed over time
   - Revelation triggered by: trust level, specific events, conversations
   - Secret revelations create events
   - Negative secrets: trust penalty, possible exile
   - Positive secrets: new capabilities revealed
   - Revelation logged in journal
5. Implement lost connections:
   - Lost connections may appear as NPCs
   - Reunion events when connection found
   - Reunion affects morale (positive or negative)
   - Some connections are dead (grief events)
   - Some connections are alive and reachable
6. Implement backstory quests:
   - Backstory creates quest hooks (find lost connection, resolve defining moment)
   - Quest hooks unique to each survivor
   - Quest completion resolves backstory arc
   - Quest rewards: morale, skills, items
7. Create backstory events:
   - "The Arrival" — new survivor arrives with backstory
   - "The Revelation" — secret backstory element revealed
   - "The Reunion" — lost connection found
   - "The Memory" — survivor shares defining moment
   - "The Past" — backstory affects current situation
   - "The Secret" — hidden backstory discovered
   - "The Resolution" — backstory arc completed
8. Add backstory quest hooks:
   - "The Doctor" — doctor survivor saves life
   - "The Soldier" — soldier survivor protects shelter
   - "The Reunion" — find survivor's lost connection
   - "The Secret" — reveal and deal with dark secret
   - "The Past" — survivor's defining moment repeats
   - "The Legacy" — survivor's backstory influences next generation
   - "The Story" — collect 10 unique survivor backstories
9. Implement backstory UI:
   - Survivor detail: backstory tab showing occupation, experiences, defining moment
   - Backstory reveal: notification when element revealed
   - Lost connections: list of missing people
   - Secrets: revealed secrets shown
   - Backstory log: all discovered backstories
10. Add backstory journal: automatic log of backstory events
11. Implement backstory tutorial: first survivor with backstory explains system
12. Add backstory tooltips: hover over backstory element shows effects
13. Create 10 occupation templates + 20 life experiences in data file

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `SurvivorLifecycle`: backstory generated at survivor creation
2. Connect to `SurvivorRelationsSystem`: backstory affects relationships
3. Integrate with `SkillProgressionSystem`: starting bonuses from backstory
4. Connect to `MoralChoiceSystem`: backstory secrets create moral choices
5. Wire into `QuestSystem`: backstory quest hooks feed into quest system
6. Connect to `JournalSystem`: backstory events logged
7. Implement old-save compatibility: existing survivors get generated backstories
8. Add deterministic seeding: generation uses `ISeededRng`
9. Create exploit prevention: backstories are random, cannot be chosen
10. Add tests: backstory generation, starting bonuses, revelation, save round-trip
11. Verify catalog integrity: all template/experience IDs resolve
12. Test edge cases: no backstory (default), complex backstory (many elements)
13. Verify headless behavior: backstory processes correctly without UI
14. Add data-integrity-selftest: backstory templates validate against skill/trait catalogs
15. Create `--backstory-selftest` verb for CI validation

## State / System Interaction Model

```text
Procedural survivor backstories
├─ Backstory generation
│  ├─ 10 occupations (doctor, soldier, farmer, etc.)
│  ├─ 20 life experiences (combat, medical, etc.)
│  ├─ Defining moment (key life event)
│  ├─ Lost connections (0-3 people)
│  ├─ Reason for survival (motivation)
│  └─ Secrets (0-2 hidden elements)
├─ Starting bonuses
│  ├─ Occupation → skill bonuses
│  ├─ Experiences → additional bonuses
│  ├─ Starting possessions
│  └─ Trait modifiers
├─ Backstory-driven behavior
│  ├─ Work preference from occupation
│  ├─ Skill checks from experiences
│  ├─ Stress response from defining moment
│  ├─ Morale from reason for survival
│  └─ Relationships from lost connections
├─ Backstory revelation
│  ├─ Hidden elements revealed over time
│  ├─ Trust affects revelation speed
│  ├─ Secret revelations create events
│  └─ Positive/negative secrets
└─ Integration
   ├─ Lifecycle (generation at creation)
   ├─ Relations (backstory affects bonds)
   ├─ Skills (starting bonuses)
   ├─ Moral choice (secrets)
   ├─ Quests (backstory hooks)
   └─ Journal (event logging)
```

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --backstory-selftest
```

## Risk

**LOW** — Backstory system is additive (enhances existing survivors) with clear inputs (generation rules) and outputs (starting bonuses, behavior modifiers). Risk of backstories feeling like flavor text rather than mechanical depth. Mitigation: ensure backstory elements have clear mechanical effects, show backstory-driven behavior in gameplay, and make revelation events meaningful.

## Definition of Done

- `BackstorySystem.cs` exists with full `CaptureState/RestoreState`
- 10 occupation templates implemented
- 20 life experiences implemented
- Procedural backstory generation functional
- Starting skill/trait bonuses from backstory
- Backstory-driven behavior (work preference, stress response)
- Backstory revelation system (secrets, trust-based)
- Lost connections mechanic (reunions, grief)
- Backstory events and quest hooks
- Save/load round-trip tested
- Deterministic generation verified
- Old saves get generated backstories
- 10 occupations + 20 experiences in data authority
- UI survivor detail with backstory tab
- Cross-system integration (lifecycle, relations, skills, moral choice, quests, journal)

## Follow-On Opportunities

- Backstory specialization (unique backstory combinations)
- Backstory legacy (survivor stories remembered across campaigns)
- Backstory quests (specific backstory arcs to resolve)
- Backstory trading (trade survivors with known backstories)
- Backstory generation control (influence backstory parameters)
