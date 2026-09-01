# Plan 166 — Shelter Identity & Naming System

## Goal

Create a shelter identity and naming system where the player names their shelter, chooses a shelter type/origin story, and develops a community identity that shapes NPC perceptions, faction interactions, and the shelter's reputation in the wasteland. Currently the shelter has no name, no identity, no origin story — it is a generic "shelter" referenced in UI and journal entries. This plan gives the player's community a name and a story, making it feel like *their* place.

## Why

**Repository evidence:** Grep for `ShelterName`, `shelter_identity`, `ShelterCustom` returns ZERO matches across the entire codebase. Journal entries, feedback messages, and UI panels all reference "the shelter" generically. `CampaignConsequenceLedger` tracks flags but not community identity. `ShelterArchiveSystem` (Plan 162) will record history but has no shelter name to attach it to. The shelter is a functional container, not a named community.

**What is missing:** Players cannot name their shelter. There is no origin story choice. No community identity. No reputation tied to shelter name. No way for other factions/NPCs to refer to the shelter by name. The shelter has no personality beyond what the player projects onto it through gameplay.

**Why existing plans don't solve it:** No plan addresses shelter naming or identity. Plan 156 (shelter expansion) adds physical rooms but not identity. Plan 159 (governance) adds political structure but not community name. Plan 162 (archive) records history but doesn't name the community whose history it records. No plan addresses shelter identity.

**Player value:** Creates ownership (this is *my* shelter with *my* name), adds roleplay depth (origin story shapes identity), generates emergent narrative (factions refer to shelter by name), and makes the shelter feel like a real community rather than a game mechanic.

## Files / Systems to Inspect

- `Assets/Ashfall.Core/Shelter/` — shelter-related systems
- `Assets/Ashfall.Core/Flags/CampaignConsequenceLedger.cs` — consequence tracking
- `Assets/Ashfall.Core/Journal/` — journal system (references "shelter" generically)
- `Assets/Ashfall.Core/Feedback/FeedbackMessageCatalogLoader.cs` — feedback messages
- `Assets/Ashfall.Core/UI/PanelRegistryBootstrap.cs` — panel definitions
- NEW: `Assets/Ashfall.Core/Shelter/ShelterIdentitySystem.cs`
- NEW: `Assets/StreamingAssets/Data/shelter_origins.json`

## Main Task 1 — Foundation / System Contract

1. Create `ShelterIdentitySystem.cs` in `Assets/Ashfall.Core/Shelter/`
2. Define `ShelterOrigin` DTO: `originId`, `originName` (e.g., "Government Bunker", "Mining Facility", "School Basement", "Private Vault", "Improvised Cellar"), `description`, `startingBonuses` (list of modifiers), `startingDrawbacks` (list of modifiers), `flavorText`
3. Define `ShelterIdentity` DTO: `shelterName` (player-chosen), `originId`, `foundingDay`, `founderSurvivorId`, `motto` (optional player-written), `color` (shelter emblem color), `emblem` (simple emblem choice)
4. Define `ShelterReputation` DTO: `shelterName`, `reputationByFaction` (dict of factionId → reputation score), `reputationBySettlement` (dict), `knownFor` (list of tags: "traders", "raiders", "healers", "hermits"), `infamy` (0-100)
5. Define `ShelterIdentityState` DTO: identity data, reputation data, origin chosen flag, naming completed flag
6. Implement `CaptureState/RestoreState` with schema versioning
7. Define 6 shelter origins:
   - **Government Bunker**: +radiation shielding, +air filtration, -space (cramped), -surface access
   - **Mining Facility**: +mining access, +underground space, -ventilation, -water access
   - **School Basement**: +education bonus, +community space, -radiation shielding, -defense
   - **Private Vault**: +starting supplies, +luxury items, -space (very small), -community morale
   - **Improvised Cellar**: +adaptability, +surface access, -radiation shielding, -starting supplies
   - **Military Outpost**: +defense bonus, +weapons cache, -community space, -medical supplies
8. Define naming mechanics:
   - Player prompted to name shelter during onboarding (after origin selection)
   - Name can be changed later at a cost (governance action, morale penalty)
   - Name appears in journal entries, feedback messages, UI headers
   - Name used by factions when referring to player's shelter
   - Name recorded in archive (Plan 162 integration)
9. Define reputation mechanics:
   - Reputation starts neutral with all factions
   - Actions affect reputation (trade → positive, raid → negative)
   - Reputation affects trade prices, diplomatic options, refugee flow
   - Known-for tags develop based on player actions
   - Infamy tracks notoriety (raiding, betrayal, cruelty)
10. Define origin effects:
    - Origin provides starting bonuses and drawbacks
    - Origin affects shelter layout (different starting rooms)
    - Origin affects available upgrades (some upgrades origin-gated)
    - Origin flavor text appears in journal and archive
11. Add deterministic seeding: reputation calculations use `ISeededRng`
12. Wire into `GameBootstrap`: `SetupShelterIdentity`, `SaveShelterIdentity`
13. Create `ShelterOriginCatalogLoader` for origin definitions
14. Implement shelter naming UI: naming prompt during onboarding, rename action in governance
15. Create shelter identity panel: shows name, origin, reputation, emblem

## Main Task 2 — Implementation / Naming / Origins / Reputation / UI

1. Implement origin selection:
   - During onboarding, player chooses shelter origin
   - Origin choice affects starting conditions (bonuses/drawbacks applied)
   - Origin flavor text displayed
   - Origin recorded in save state
2. Implement shelter naming:
   - After origin selection, player names shelter
   - Name input with validation (length, characters)
   - Name stored in identity state
   - Name propagated to journal, feedback, UI
3. Implement motto/emblem customization:
   - Player can write shelter motto (short text)
   - Player chooses emblem color and simple emblem
   - Motto/emblem displayed in UI and archive
   - Customization optional (player can skip)
4. Implement reputation system:
   - Reputation tracked per faction and settlement
   - Actions modify reputation (trade, diplomacy, raid, betrayal)
   - Reputation affects trade prices (high rep = discount)
   - Reputation affects diplomatic options (high rep = more options)
   - Reputation affects refugee flow (good rep = more refugees)
5. Implement known-for tags:
   - Tags develop based on player actions
   - "Traders" tag from frequent trade
   - "Raiders" tag from combat/raiding
   - "Healers" tag from medical aid
   - "Hermits" tag from isolation
   - Tags affect NPC perceptions and dialogue
6. Implement infamy system:
   - Infamy increases from cruel/betraying actions
   - High infamy: factions hostile, refugees afraid
   - Infamy decays slowly over time
   - Infamy affects ending options
7. Implement name propagation:
   - Journal entries use shelter name ("The Haven", not "the shelter")
   - Feedback messages use shelter name
   - UI headers display shelter name
   - Faction dialogue references shelter name
   - Archive entries tagged with shelter name
8. Implement rename mechanic:
   - Player can rename shelter via governance action
   - Rename costs morale (community confusion)
   - Rename recorded in archive ("Formerly known as...")
   - Rename updates all references
9. Create shelter identity events:
   - "The Naming" — shelter named for the first time
   - "The Origin" — origin story discovered/remembered
   - "The Reputation" — shelter gains reputation tag
   - "The Infamy" — shelter becomes infamous
   - "The Rename" — shelter renamed
   - "The Emblem" — emblem/motto chosen
   - "The Legacy" — shelter identity established
10. Add shelter identity quest hooks:
    - "The Name" — choose shelter name (onboarding)
    - "The Origin" — choose shelter origin (onboarding)
    - "The Reputation" — build shelter reputation
    - "The Emblem" — design shelter emblem
    - "The Legacy" — establish shelter identity in wasteland
    - "The Rename" — rename shelter (governance action)
    - "The Infamy" — deal with shelter notoriety
11. Implement shelter identity UI:
    - Identity panel: name, origin, motto, emblem, reputation
    - Naming prompt during onboarding
    - Rename action in governance panel
    - Reputation display in faction panel
    - Known-for tags in identity panel
12. Add identity journal: automatic log of identity events
13. Implement identity tutorial: naming prompt explains system
14. Add identity tooltips: hover over name shows origin/reputation
15. Create 6 shelter origins in data file

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `OnboardingJourney`: origin/naming during onboarding
2. Connect to `JournalSystem`: shelter name in journal entries
3. Integrate with `FeedbackMessageCatalogLoader`: shelter name in feedback
4. Connect to `FactionBranchCoordinator`: reputation affects factions
5. Wire into `ShelterArchiveSystem` (Plan 162): shelter name in archive
6. Connect to `GovernanceSystem` (Plan 159): rename action
7. Implement old-save compatibility: existing saves get default identity ("Shelter", no origin)
8. Add deterministic seeding: reputation uses `ISeededRng`
9. Create exploit prevention: rename has cost, reputation is earned
10. Add tests: origin selection, naming, reputation changes, save round-trip
11. Verify catalog integrity: all origin IDs resolve
12. Test edge cases: no name (default), extensive reputation (many tags)
13. Verify headless behavior: identity processes correctly without UI
14. Add data-integrity-selftest: shelter origins validate against catalogs
15. Create `--shelter-identity-selftest` verb for CI validation

## State / System Interaction Model

```text
Shelter identity system
├─ Origin selection (during onboarding)
│  ├─ 6 origins with bonuses/drawbacks
│  ├─ Origin affects starting conditions
│  ├─ Origin flavor text in journal
│  └─ Origin recorded in save
├─ Shelter naming
│  ├─ Player names shelter
│  ├─ Name propagated to journal/feedback/UI
│  ├─ Name used by factions
│  └─ Name can be changed (at cost)
├─ Motto & emblem
│  ├─ Player writes motto
│  ├─ Player chooses emblem color/style
│  ├─ Displayed in UI and archive
│  └─ Optional customization
├─ Reputation system
│  ├─ Per-faction reputation score
│  ├─ Actions modify reputation
│  ├─ Reputation affects trade/diplomacy/refugees
│  └─ Known-for tags develop
├─ Infamy system
│  ├─ Infamy from cruel actions
│  ├─ High infamy: factions hostile
│  ├─ Infamy decays over time
│  └─ Infamy affects endings
└─ Integration
   ├─ Onboarding (origin/naming)
   ├─ Journal (shelter name)
   ├─ Feedback (shelter name)
   ├─ Factions (reputation)
   ├─ Archive (shelter history)
   └─ Governance (rename action)
```

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --shelter-identity-selftest
```

## Risk

**LOW** — Shelter identity is straightforward with clear inputs (origin choice, name, actions) and outputs (reputation, identity). Risk of naming feeling cosmetic rather than meaningful. Mitigation: tie reputation to gameplay (trade, diplomacy, refugees), show name in journal/feedback, and integrate with faction interactions.

## Definition of Done

- `ShelterIdentitySystem.cs` exists with full `CaptureState/RestoreState`
- 6 shelter origins implemented with bonuses/drawbacks
- Shelter naming functional (onboarding + rename)
- Motto and emblem customization working
- Reputation system with per-faction scores
- Known-for tags develop from player actions
- Infamy system functional
- Name propagated to journal, feedback, UI, factions
- Shelter identity events and quest hooks
- Save/load round-trip tested
- Deterministic reputation verified
- Old saves load without error (default identity)
- 6 shelter origins in data authority
- UI panel showing identity, reputation, emblem
- Cross-system integration (onboarding, journal, feedback, factions, archive, governance)

## Follow-On Opportunities

- Shelter emblem generator (procedural emblem creation)
- Shelter reputation events (reputation milestones)
- Shelter identity legacy (identity carries to New Game+)
- Shelter identity quests (build reputation, deal with infamy)
- Shelter identity competitions (famous shelters remembered)
