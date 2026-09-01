# Plan 204 — Survivor Recruitment & Defection System

## Goal

Create a survivor recruitment and defection system where the player can actively recruit new survivors from factions, wilderness, and other settlements — and where faction members can defect to join the player's shelter. Currently survivors arrive through narrative events (`EventSystem`) and procedural backstories (`Plan_174`) — but there is no active recruitment system, no defection mechanics, no recruitment missions, no faction member poaching, no structured survivor acquisition. New survivors appear but the player has no agency in growing their population. This plan makes population growth a strategic gameplay layer.

## Why

**Repository evidence:** Grep for `RecruitmentSystem`, `DefectorSystem`, `DefectSystem`, `RecruitSurvivor`, `NewSurvivorRecruitment`, `FactionDefector`, `RecruitmentMission`, `PoachingSystem` in Core returns ZERO matches. Survivors are created through narrative events and procedural generation. Plan 174 (Procedural Survivor Backstories) generates backstories for new survivors. Plan 168 (Propaganda) mentions "recruit_defectors" as a campaign objective. Plan 153 (Faction Espionage) mentions "The Defector" as a quest hook. But no dedicated recruitment/defection system exists.

**What is missing:** No active recruitment system. No defection mechanics. No recruitment missions. No faction member poaching. No structured survivor acquisition. No recruitment costs/resources. No recruitment success/failure. New survivors appear through events but player has no agency in growing population.

**Why existing plans don't solve it:** Plan 174 (procedural backstories) generates backstories but doesn't add recruitment mechanics. Plan 168 (propaganda) mentions defection as campaign objective but doesn't implement. Plan 153 (espionage) mentions defector quest hook but doesn't implement. Plan 138 (shelter defense) mentions visitors seeking shelter but not active recruitment. No plan addresses recruitment/defection as a system.

**Player value:** Creates strategic depth (grow population intentionally), adds faction interaction (poach members, accept defectors), generates emergent stories (dramatic defections, failed recruitment), and makes population growth a meaningful gameplay choice rather than random event.

## Files / Systems to Inspect

- `Assets/Ashfall.Core/Survivors/` — survivor management
- `Assets/Ashfall.Core/Factions/FactionStanceEngine.cs` — faction trust
- `Assets/Ashfall.Core/Factions/FactionBranchCoordinator.cs` — faction branches
- `Assets/Ashfall.Core/EventSystem.cs` — narrative events
- `Assets/Ashfall.Core/ExpeditionSystem.cs` — expeditions (recruitment opportunities)
- NEW: `Assets/Ashfall.Core/Survivors/RecruitmentSystem.cs`
- NEW: `Assets/StreamingAssets/Data/recruitment_templates.json`

## Main Task 1 — Foundation / System Contract

1. Create `RecruitmentSystem.cs` in `Assets/Ashfall.Core/Survivors/`
2. Define `RecruitmentCampaign` DTO: `campaignId`, `campaignType` (active_recruitment/defection_inducement/asylum_offer/forced_recruitment/trade_for_survivor), `targetFaction` (faction_id or null for wilderness), `targetSurvivor` (survivor_id or null for general recruitment), `assignedRecruiter` (survivor_id), `duration` (days), `successChance` (0-100), `cost` (resources required), `status` (planned/in_progress/succeeded/failed/cancelled), `startedDay`, `completedDay` (-1 if incomplete)
3. Define `RecruitmentCandidate` DTO: `candidateId`, `candidateType` (faction_member/wilderness_survivor/refugee/trader/defector/prisoner), `currentFaction` (faction_id or null), `location` (location_id), `willingness` (0-100, how open to joining), `requirements` (list of conditions: resources, safety, relationships), `skills` (list of skills), `traits` (list of traits), `backstory` (backstory_id), `discoveredDay` (-1 if unknown)
4. Define `DefectionOffer` DTO: `offerId`, `targetSurvivorId` (faction member being recruited), `offerType` (asylum/money/ideology/revenge/family/protection), `offerValue` (resources or promises), `factionReaction` (how faction responds), `successChance` (0-100), `discoveredChance` (0-100, chance faction discovers), `status` (pending/accepted/rejected/discovered)
5. Define `RecruitmentEvent` DTO: `eventId`, `eventType` (recruitment_success/recruitment_failure/defection_offer/defection_accepted/defection_discovered/recruiter_captured/recruiter_killed/survivor_arrived), `day`, `description`, `participants` (list of survivor_ids), `outcome` (success/failure/partial), `consequences` (list of effects)
6. Define `RecruitmentState` DTO: list of active recruitment campaigns, list of known candidates, list of defection offers, list of recruitment events, recruitment settings (max active campaigns, auto-discover candidates bool)
7. Implement `CaptureState/RestoreState` with schema versioning
8. Define recruitment types (5+ types):
   - **Active Recruitment**: send recruiter to find/recruit survivors in wilderness or settlements
   - **Defection Inducement**: offer faction member reasons to defect (money, ideology, safety)
   - **Asylum Offer**: offer shelter to refugees, persecuted individuals, deserters
   - **Forced Recruitment**: capture/prisoner recruitment (ethical implications)
   - **Trade for Survivor**: exchange resources/items with faction for one of their members
9. Define candidate types (6+ types):
   - **Faction Member**: active faction member, can be recruited through defection
   - **Wilderness Survivor**: lone survivor in wasteland, can be recruited through expedition
   - **Refugee**: displaced person seeking shelter, can be offered asylum
   - **Trader**: traveling merchant, can be recruited (or trade for their services)
   - **Defector**: faction member actively seeking to leave, approaches player
   - **Prisoner**: captured individual, can be recruited (ethical implications)
10. Define recruitment mechanics:
    - Player assigns recruiter survivor to campaign
    - Campaign takes time (travel + negotiation)
    - Success based on: recruiter skill + target willingness + faction trust + offer value
    - Failed recruitment: recruiter returns empty-handed, possible faction hostility
    - Successful recruitment: new survivor arrives at shelter
    - Recruitment logged
11. Define defection mechanics:
    - Faction members can be induced to defect
    - Defection offer: player offers reasons to leave faction
    - Success based on: faction member's loyalty + player's offer + faction morale
    - Defection discovered: faction hostility, recruiter in danger
    - Successful defection: faction member joins shelter
    - Defection logged
12. define recruitment costs:
    - Active recruitment: resources (food, water, equipment) + recruiter time
    - Defection inducement: resources (money, items, promises) + risk
    - Asylum offer: resources (shelter space, food, integration)
    - Forced recruitment: resources (capture equipment, guards) + ethical cost
    - Trade: resources/items exchanged for survivor
13. Define recruitment consequences:
    - Successful recruitment: new survivor, faction trust change
    - Failed recruitment: wasted resources, possible faction hostility
    - Defection discovered: faction hostility, recruiter captured/killed
    - Forced recruitment: morale impact, ethical consequences
    - Recruitment affects faction relations
14. Define recruitment integration:
    - New survivors need integration (orientation, assignment, relationships)
    - New survivors have backstory, skills, traits
    - New survivors may have grudges against factions
    - New survivors integrate with existing shelter population
    - Integration logged
15. Add deterministic seeding: recruitment outcomes use `ISeededRng`
16. Wire into `GameBootstrap`: `SetupRecruitment`, `TickRecruitment`, `SaveRecruitment`

## Main Task 2 — Implementation / Campaigns / Candidates / Defection / Integration / UI

1. Implement recruitment campaigns:
   - Player creates campaign (type, target, recruiter)
   - Campaign takes time (travel + negotiation)
   - Campaign success check (recruiter skill + willingness + trust + offer)
   - Campaign outcome (success/failure)
   - Campaign logged
2. Implement candidate discovery:
   - Candidates discovered through expeditions, events, intelligence
   - Candidates have type, location, willingness, requirements
   - Candidates can be recruited (if discovered)
   - Candidate discovery logged
3. Implement defection offers:
   - Player makes defection offer to faction member
   - Offer type (asylum, money, ideology, revenge, family, protection)
   - Offer value (resources or promises)
   - Success check (loyalty + offer + faction morale)
   - Faction discovery check (chance faction learns of offer)
   - Defection outcome (accepted/rejected/discovered)
   - Defection logged
4. Implement forced recruitment:
   - Player captures prisoner
   - Prisoner can be recruited (ethical implications)
   - Recruitment success based on: prisoner morale, treatment, time
   - Forced recruitment: morale impact on shelter
   - Forced recruitment logged
5. Implement trade for survivor:
   - Player offers resources/items to faction for survivor
   - Faction evaluates offer (value + relationship + survivor importance)
   - Trade accepted/rejected
   - Trade logged
6. implement recruitment integration:
   - New survivor arrives at shelter
   - Integration period (orientation, assignment)
   - New survivor meets existing survivors
   - Relationships form
   - Integration logged
7. Implement recruitment UI:
   - Recruitment panel: active campaigns, known candidates
   - Campaign detail: type, target, recruiter, progress, success chance
   - Candidate detail: type, location, willingness, requirements, skills
   - Defection panel: active offers, faction reactions
   - Integration panel: new survivors, integration progress
   - Recruitment log: history of campaigns/outcomes
8. Create recruitment events:
    - "The Recruitment" — recruitment campaign launched
    - "The Defector" — faction member offers to defect
    - "The Arrival" — new survivor arrives
    - "The Failure" — recruitment campaign failed
    - "The Discovery" — defection offer discovered by faction
    - "The Capture" — recruiter captured
    - "The Trade" — survivor traded from faction
    - "The Integration" — new survivor integrated
9. Add recruitment quest hooks:
    - "The Recruiter" — successfully recruit 10 survivors
    - "The Diplomat" — induce 5 faction defections
    - "The Humanitarian" — offer asylum to 10 refugees
    - "The Trader" — trade for 3 survivors
    - "The Leader" — build shelter population to 20
    - "The Integrator" — successfully integrate 15 new survivors
    - "The Network" — discover 20 recruitment candidates
10. Implement recruitment tutorial: first recruitment campaign explains system
11. Add recruitment tooltips: hover over campaign/candidate shows details
12. Create recruitment templates in data file (10+ campaign types, 20+ candidate templates)
13. Implement recruitment persistence: campaigns/candidates saved with game state
14. Integrate with `FactionStanceEngine`: recruitment affects faction trust

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `FactionStanceEngine`: recruitment affects faction trust
2. Connect to `FactionBranchCoordinator`: defection affects branch decisions
3. Integrate with `ExpeditionSystem`: expeditions discover candidates
4. Connect to `EventSystem`: recruitment events integrate with narrative
5. Wire into `SurvivorRelationsSystem`: new survivors form relationships
6. Connect to `SkillProgressionSystem`: new survivors have skills
7. Connect to `TraitSystem`: new survivors have traits
8. Implement old-save compatibility: existing saves get no active campaigns
9. Add deterministic seeding: recruitment outcomes use `ISeededRng`
10. Create exploit prevention: recruitment is probabilistic, can't be gamed
11. Add tests: recruitment campaigns, candidate discovery, defection offers, integration, save round-trip
12. Verify all recruitment types work correctly
13. Test edge cases: no recruitment (current behavior), heavy recruitment (population boom)
14. Verify headless behavior: recruitment processes correctly without UI
15. Add data-integrity-selftest: recruitment validates against faction/survivor/location catalogs
16. Create `--recruitment-selftest` verb for CI validation

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --recruitment-selftest
```

## Risk

**LOW** — Recruitment is straightforward with clear inputs (campaigns, offers) and outputs (new survivors, faction reactions). Risk of recruitment feeling like a slot machine. Mitigation: make recruitment meaningful (costs, consequences), show clear cause-effect, and ensure new survivors feel like individuals not just stat blocks.

## Definition of Done

- `RecruitmentSystem.cs` exists with full `CaptureState/RestoreState`
- 5+ recruitment types (active, defection, asylum, forced, trade)
- 6+ candidate types (faction member, wilderness, refugee, trader, defector, prisoner)
- Recruitment campaign mechanics (recruiter, duration, success chance, cost)
- Defection offer mechanics (offer type, value, success, discovery risk)
- Recruitment costs and consequences
- New survivor integration mechanics
- Recruitment events and quest hooks
- Save/load round-trip tested
- Deterministic recruitment outcomes verified
- Old saves load with no active campaigns
- Recruitment templates in data authority
- UI recruitment panel, campaign detail, candidate detail, defection panel, integration panel, log
- Cross-system integration (faction stance, branch coordinator, expedition, events, relations, skills, traits)

## Follow-On Opportunities

- Recruitment specialization (survivors become expert recruiters/diplomats)
- Recruitment legacy (famous recruits remembered)
- Recruitment quests (specific recruitment goals)
- Recruitment events (mass defection, legendary recruiter)
- Recruitment trading (trade recruitment services with other settlements)
