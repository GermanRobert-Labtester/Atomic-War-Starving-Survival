# Plan 207 — Shelter Reputation & External Perception System

## Goal

Create a shelter reputation and external perception system where the shelter as a whole is known by the outside world — factions, traders, refugees, raiders — and where reputation affects visitor quality, trade opportunities, attack frequency, diplomatic options, and recruitment prospects. Currently `FactionStanceEngine.cs` (172 lines) tracks per-faction trust, and Plan 138 mentions "shelter reputation" as a factor for visitor quality — but there is no dedicated reputation system, no external perception tracking, no reputation mechanics, no fame/infamy, no reputation decay or growth. The shelter is anonymous to the outside world. This plan makes the shelter a known entity in the wasteland.

## Why

**Repository evidence:** Grep for `ShelterReputation`, `ReputationSystem`, `ExternalPerception`, `BunkerReputation`, `WorldReputation`, `BunkerFame`, `InfamySystem` in Core returns ZERO matches. Plans 138 (shelter defense), 166 (shelter identity), and 168 (propaganda) mention "shelter reputation" as a concept that affects things — but none implement it as a system. `FactionStanceEngine` (172 lines) tracks per-faction trust, which is bilateral (faction↔player), not the shelter's global reputation in the wasteland.

**What is missing:** No shelter reputation system. No external perception tracking. No fame/infamy mechanics. No reputation decay or growth. No reputation effects on visitors, trade, attacks, diplomacy. The shelter has no identity beyond what the player knows internally.

**Why existing plans don't solve it:** Plan 138 (shelter defense) mentions reputation as a visitor-quality factor but doesn't implement it. Plan 166 (shelter identity/naming) adds shelter naming but not reputation. Plan 168 (propaganda) mentions reputation damage from detected propaganda but doesn't implement. Plan 197 (faction diplomacy) adds treaties but not global reputation. No plan addresses shelter reputation as a system.

**Player value:** Creates strategic identity (shelter is known for something), adds consequences to actions (raiding factions → feared reputation, helping refugees → respected reputation), generates emergent dynamics (reputation attracts visitors/trade or attacks), and makes the shelter feel like a real place in a real world.

## Files / Systems to Inspect

- `Assets/Ashfall.Core/Factions/FactionStanceEngine.cs` — per-faction trust (complementary)
- `Assets/Ashfall.Core/AirlockSecuritySystem.cs` — visitor arrivals
- `Assets/Ashfall.Core/HoldfastTradeSession.cs` — trade sessions
- `Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs` — morality (reputation driver)
- NEW: `Assets/Ashfall.Core/ShelterReputationSystem.cs`
- NEW: `Assets/StreamingAssets/Data/reputation_events.json`

## Main Task 1 — Foundation / System Contract

1. Create `ShelterReputationSystem.cs` in `Assets/Ashfall.Core/`
2. Define `ShelterReputationState` DTO: `overallReputation` (0-100, composite), `reputationByFaction` (dict of faction_id → reputation score), `reputationTags` (list of active reputation tags), `reputationHistory` (list of reputation events), `reputationDecay` (daily decay rate), `lastReputationEvent` (day), `notoriety` (0-100, how well-known the shelter is), `settings` (reputation enabled bool, decay rate, notoriety growth rate)
3. Define `ReputationTag` DTO: `tagId`, `tagName` (feared/respected/generous/cruel/reliable/treacherous/strong/weak/rich/poor/sanctuary/dangerous), `magnitude` (0-100, how strongly this tag applies), `accumulatedDay`, `decayRate` (per day), `source` (what caused this tag)
4. Define `ReputationEvent` DTO: `eventId`, `eventType` (raid_defended/raid_lost/refugee_accepted/refugee_rejected/trade_completed/trade_cheated/faction_member_killed/faction_aided_propaganda_detected/notable_survivor_death/legendary_discovery/humanitarian_act/atrocity), `day`, `description`, `reputationChange` (float, positive or negative), `notorietyChange` (float), `tagsAffected` (list of tag_ids), `factionAffected` (faction_id or null for global)
5. Define `ReputationEffect` DTO: `effectId`, `effectType` (visitor_quality/trade_terms/attack_frequency/diplomatic_options/recruitment_success/defection_chance/ally_requests/tribute_demands), `reputationThreshold` (minimum reputation for effect), `tagRequired` (reputation tag required, or null), `magnitude` (how strongly effect applies), `description`
6. Define `FactionReputation` DTO: `factionId`, `reputationScore` (0-100, this faction's view of shelter), `tags` (list of faction-specific tags), `lastInteraction` (day), `trustModifier` (float, applied to FactionStanceEngine trust)
7. Implement `CaptureState/RestoreState` with schema versioning
8. Define reputation tags (12+ types):
   - **Feared**: shelter has repelled attacks, killed raiders, shown strength
   - **Respected**: shelter honors agreements, treats visitors fairly
   - **Generous**: shelter accepts refugees, shares resources
   - **Cruel**: shelter rejects refugees in need, executes prisoners
   - **Reliable**: shelter honors trade agreements, delivers on promises
   - **Treacherous**: shelter breaks agreements, betrays allies
   - **Strong**: shelter has large population, good defenses, wins combats
   - **Weak**: shelter has small population, poor defenses, loses combats
   - **Rich**: shelter has abundant resources, valuable trade goods
   - **Poor**: shelter lacks resources, desperate trade
   - **Sanctuary**: shelter accepts refugees, provides medical aid
   - **Dangerous**: shelter is aggressive, raids neighbors, hostile
9. Define reputation drivers:
   - **Combat outcomes**: defending raids (+feared, +respected), losing raids (+weak), attacking others (+dangerous, +feared)
   - **Refugee treatment**: accepting refugees (+generous, +sanctuary, +respected), rejecting (+cruel if desperate)
   - **Trade behavior**: fair trade (+reliable, +respected), cheating (+treacherous)
   - **Faction interaction**: aiding factions (+respected by faction), killing faction members (-reputation with faction)
   - **Propaganda**: detected propaganda (-reputation, +treacherous)
   - **Notable events**: legendary discoveries (+notoriety), notable deaths (+notoriety)
   - **Humanitarian acts**: medical aid (+sanctuary, +generous), atrocities (+cruel, +dangerous)
10. Define reputation effects:
    - **Visitor quality**: high reputation → better visitors (skilled, friendly); low → desperate or hostile
    - **Trade terms**: good reputation → better prices, more options; bad → worse prices, fewer options
    - **Attack frequency**: feared reputation → fewer raids; weak reputation → more raids
    - **Diplomatic options**: respected reputation → more treaty options; treacherous → fewer
    - **Recruitment success**: good reputation → easier to recruit; bad → harder
    - **Defection chance**: respected → faction members more likely to defect to you
    - **Ally requests**: respected → allies ask for help (quest opportunities)
    - **Tribute demands**: feared → weaker factions may offer tribute
11. Define notoriety mechanics:
    - Notoriety tracks how well-known the shelter is (0-100)
    - Notoriety increases from: notable events, combat, trade, propaganda
    - High notoriety: more visitors (good and bad), more opportunities, more threats
    - Low notoriety: shelter is unknown, fewer interactions
    - Notoriety decays slowly if shelter is inactive
12. Define reputation decay:
    - Reputation tags decay over time (actions have consequences but fade)
    - Overall reputation drifts toward neutral if no recent events
    - Faction-specific reputation decays if no interaction
    - Decay rate configurable
13. Add deterministic seeding: reputation events use `ISeededRng`
14. Wire into `GameBootstrap`: `SetupShelterReputation`, `TickShelterReputation`, `SaveShelterReputation`

## Main Task 2 — Implementation / Tags / Events / Effects / Notoriety / UI

1. Implement reputation tracking:
   - Overall reputation (0-100, composite)
   - Per-faction reputation
   - Reputation tags with magnitude and decay
   - Reputation history
   - Tracking logged
2. Implement reputation events:
   - Events trigger reputation changes
   - Each event: type, reputation change, notoriety change, tags affected
   - Events logged
3. Implement reputation effects:
   - Reputation affects visitor quality
   - Reputation affects trade terms
   - Reputation affects attack frequency
   - Reputation affects diplomatic options
   - Effects calculated from reputation + tags
4. Implement notoriety:
   - Notoriety tracks how well-known shelter is
   - Notoriety affects interaction frequency
   - Notoriety decays if inactive
   - Notoriety logged
5. Implement reputation decay:
   - Tags decay over time
   - Overall reputation drifts toward neutral
   - Faction reputation decays without interaction
   - Decay logged
6. Implement reputation UI:
   - Reputation panel: overall reputation, notoriety, active tags
   - Tag detail: magnitude, decay rate, source
   - Faction reputation: per-faction scores
   - Reputation log: history of events
   - Effects panel: active reputation effects
   - Reputation map: show how different factions view shelter
7. Create reputation events:
    - "The Legend" — shelter becomes well-known
    - "The Fear" — shelter becomes feared
    - "The Respect" — shelter earns respect
    - "The Betrayal" — reputation damaged by treachery
    - "The Sanctuary" — shelter known as refuge
    - "The Atrocity" — shelter known for cruelty
    - "The Fade" — reputation forgotten
    - "The Notoriety" — shelter becomes famous/infamous
8. Add reputation quest hooks:
    - "The Legend" — reach 90+ notoriety
    - "The Feared" — become feared by 5 factions
    - "The Respected" — become respected by 5 factions
    - "The Sanctuary" — accept 20 refugees
    - "The Trader" — maintain reliable reputation for 100 days
    - "The Infamous" — reach 80+ feared tag
    - "The Unknown" — maintain low notoriety for 200 days
9. Implement reputation tutorial: first reputation event explains system
10. Add reputation tooltips: hover over tag shows details
11. Create reputation event definitions in data file (20+ event types)
12. Implement reputation persistence: reputation saved with game state
13. Integrate with `FactionStanceEngine`: reputation modifies faction trust

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `FactionStanceEngine`: reputation modifies faction trust
2. Connect to `AirlockSecuritySystem`: reputation affects visitor quality
3. Integrate with `HoldfastTradeSession`: reputation affects trade terms
4. Connect to `MoralChoiceSystem`: moral choices drive reputation
5. Wire into `ExpeditionSystem`: reputation affects faction encounters
6. Connect to `RecruitmentSystem` (Plan 204): reputation affects recruitment success
7. Connect to `FactionDiplomacySystem` (Plan 197): reputation affects diplomatic options
8. Implement old-save compatibility: existing saves get neutral reputation, zero notoriety
9. Add deterministic seeding: reputation events use `ISeededRng`
10. Create exploit prevention: reputation is event-driven, can't be gamed
11. Add tests: reputation events, tag accumulation, decay, effects, notoriety, save round-trip
12. Verify all reputation tags work correctly
13. Test edge cases: no reputation (current behavior), extreme reputation (feared/respected)
14. Verify headless behavior: reputation processes correctly without UI
15. Add data-integrity-selftest: reputation validates against faction catalogs
16. Create `--shelter-reputation-selftest` verb for CI validation

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --shelter-reputation-selftest
```

## Risk

**LOW** — Reputation is straightforward with clear inputs (events) and outputs (tags, effects). Risk of reputation feeling like an abstract number. Mitigation: make effects tangible (better visitors, worse trade, fewer raids), show clear cause-effect, and ensure reputation feels earned not arbitrary.

## Definition of Done

- `ShelterReputationSystem.cs` exists with full `CaptureState/RestoreState`
- 12+ reputation tags (feared, respected, generous, cruel, reliable, treacherous, strong, weak, rich, poor, sanctuary, dangerous)
- Reputation events (combat, refugee, trade, faction, propaganda, notable, humanitarian)
- Reputation effects (visitor quality, trade terms, attack frequency, diplomacy, recruitment, defection, allies, tribute)
- Notoriety system (how well-known shelter is)
- Reputation decay (tags fade, reputation drifts neutral)
- Per-faction reputation tracking
- Reputation events and quest hooks
- Save/load round-trip tested
- Deterministic reputation events verified
- Old saves load with neutral reputation, zero notoriety
- Reputation event definitions in data authority (20+ types)
- UI reputation panel, tag detail, faction reputation, log, effects panel, reputation map
- Cross-system integration (faction stance, airlock security, trade session, moral choice, expedition, recruitment, diplomacy)

## Follow-On Opportunities

- Reputation specialization (survivors become expert diplomats/propagandists)
- Reputation legacy (famous shelters remembered across campaigns)
- Reputation quests (specific reputation goals)
- Reputation events (legendary reputation, catastrophic reputation collapse)
- Reputation trading (trade reputation services with other settlements)
