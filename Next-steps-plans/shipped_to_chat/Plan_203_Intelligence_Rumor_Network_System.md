# Plan 203 — Intelligence & Rumor Network System

## Goal

Create an intelligence gathering and rumor network system where the player can build an informant network, gather intelligence on faction movements, spread/counter rumors, and use information as a strategic resource. Currently `FactionStanceEngine.cs` tracks faction trust, `SignalTriangulationSystem.cs` handles radio signal analysis, and Plan 131 (Information Network) mentions propaganda — but there is no dedicated intelligence/rumor system, no informant network, no rumor spreading mechanics, no intelligence gathering operations, no information-as-resource gameplay. Information flows through narrative events but isn't a player-managed system. This plan makes intelligence a strategic gameplay layer.

## Why

**Repository evidence:** Grep for `IntelligenceSystem`, `RumorSystem`, `InformationNetwork`, `IntelSystem`, `RumorMill`, `SpySystem`, `InformantSystem`, `IntelGathering` in Core returns ZERO matches. `FactionStanceEngine.cs` (172 lines) tracks trust/trade stances. `SignalTriangulationSystem.cs` handles radio signals. Plan 131 (Information Network) mentions propaganda campaigns. Plan 153 (Faction Espionage) mentions informant mechanics. Plan 168 (Propaganda) mentions rumor campaigns. But no dedicated intelligence/rumor system exists.

**What is missing:** No intelligence gathering system. No informant network. No rumor spreading/countering mechanics. No intelligence operations (surveillance, interception, analysis). No information quality/reliability tracking. No intelligence-as-resource gameplay. Information arrives through narrative events but isn't a player-managed strategic system.

**Why existing plans don't solve it:** Plan 131 (information network) adds information flow but not intelligence gathering. Plan 153 (espionage) adds sabotage but not intelligence network. Plan 168 (propaganda) adds rumor campaigns but not rumor mechanics. No plan addresses intelligence/rumor as a player-managed system.

**Player value:** Creates strategic depth (information is power), adds espionage gameplay (build spy networks, gather intel), generates emergent stories (double agents, false rumors, intelligence failures), and makes faction interactions more strategic (know your enemy).

## Files / Systems to Inspect

- `Assets/Ashfall.Core/Factions/FactionStanceEngine.cs` — faction trust
- `Assets/Ashfall.Core/Radio/SignalTriangulationSystem.cs` — radio signals
- `Assets/Ashfall.Core/ExpeditionSystem.cs` — expeditions (intel gathering)
- `Assets/Ashfall.Core/HoldfastTradeSession.cs` — trade (intel trading)
- NEW: `Assets/Ashfall.Core/Intelligence/IntelligenceNetworkSystem.cs`
- NEW: `Assets/StreamingAssets/Data/intelligence_templates.json`

## Main Task 1 — Foundation / System Contract

1. Create `IntelligenceNetworkSystem.cs` in `Assets/Ashfall.Core/Intelligence/`
2. Define `IntelligenceNetwork` DTO: `networkId`, `networkName`, `informants` (list of informant records), `intelligenceReports` (list of reports), `rumors` (list of active rumors), `networkReputation` (0-100, how trustworthy network is), `counterIntelligenceLevel` (0-100, protection against enemy intel), `lastOperationDay`, `networkSettings` (auto-gather bool, rumor propagation bool)
3. Define `Informant` DTO: `informantId`, `survivorId` (or faction_npc_id), `informantType` (field_agent/placed_spy/trade_contact/local_source/defector), `coverageArea` (faction_id or location_id or region), `reliability` (0-100, how accurate their intel is), `accessLevel` (0-100, how deep their access is), `coverStatus` (intact/suspicious/compromised/blown), `lastContactDay`, `paymentPerDay` (resources required), `motive` (money/ideology/coercion/revenge)
4. Define `IntelligenceReport` DTO: `reportId`, `reportType` (faction_movement/military_strength/resource_cache/leadership_change/plot_discovered/terrain_intel/trade_intel), `source` (informant_id or operation_id), `subject` (faction_id or location_id or survivor_id), `content` (description of intelligence), `reliability` (0-100, source reliability), `verification` (unverified/partially_verified/verified/false), `receivedDay`, `expiresDay` (-1 if permanent), `actionable` bool
5. Define `Rumor` DTO: `rumorId`, `rumorType` (faction_rumor/shelter_rumor/person_rumor/location_rumor/event_rumor), `origin` (source faction_id or survivor_id or location_id), `content` (rumor description), `spreadLevel` (0-100, how widely spread), `credibility` (0-100, how believable), `targetFaction` (faction affected), `targetSurvivor` (survivor affected), `plantedDay`, `lastSpreadDay`, `isCounterRumor` bool (planted to counter another rumor)
6. Define `IntelligenceOperation` DTO: `operationId`, `operationType` (surveillance/interception/infiltration/extraction/assassination/sabotage/recruitment), `target` (faction_id or location_id or survivor_id), `assignedAgents` (list of survivor_ids), `duration` (days), `successChance` (0-100), `risk` (0-100), `status` (planned/in_progress/completed/failed/compromised), `startedDay`, `completedDay` (-1 if incomplete)
7. Define `CounterIntelligence` DTO: `counterIntelId`, `threatType` (enemy_spy/information_leak/propaganda_campaign/assassination_plot/sabotage_plot), `detectedDay`, `threatLevel` (0-100), `source` (how detected), `investigationStatus` (undetected/suspected/investigating/identified/neutralized), `responsibleFaction` (faction_id if external threat)
8. Define `IntelligenceNetworkState` DTO: list of informant networks, list of intelligence reports, list of active rumors, list of operations, list of counter-intelligence threats, intelligence settings (network activity level, rumor auto-spread bool)
9. Implement `CaptureState/RestoreState` with schema versioning
10. Define informant types (5+ types):
    - **Field Agent**: survivor sent to gather intel in field, high risk, high access
    - **Placed Spy**: long-term infiltrator in faction, deep access, very high risk
    - **Trade Contact**: trader who shares rumors/intel, low risk, low access
    - **Local Source**: local resident who observes and reports, low risk, limited access
    - **Defector**: faction member who switched sides, high access, variable reliability
11. Define intelligence report types (7+ types):
    - **Faction Movement**: troop movements, patrol routes, military operations
    - **Military Strength**: faction military capabilities, equipment, numbers
    - **Resource Cache**: location of supplies, weapons, valuable items
    - **Leadership Change**: faction leadership shifts, internal politics
    - **Plot Discovered**: enemy plans against player or allies
    - **Terrain Intel**: map information, hidden locations, hazards
    - **Trade Intel**: trade routes, market conditions, economic opportunities
12. Define rumor mechanics:
    - Rumors spread through factions and settlements
    - Rumor credibility affects how seriously they're taken
    - Player can plant rumors (disinformation)
    - Player can counter enemy rumors
    - Rumors affect faction trust, morale, decisions
    - Rumors have spread level (local → regional → widespread)
13. Define intelligence operations:
    - **Surveillance**: observe target, gather intel, low risk
    - **Interception**: intercept communications, medium risk
    - **Infiltration**: place spy in faction, high risk
    - **Extraction**: rescue compromised agent, high risk
    - **Recruitment**: turn enemy agent to your side, very high risk
14. Define counter-intelligence:
    - Enemy factions gather intel on player
    - Enemy factions plant spies in shelter
    - Player can detect and neutralize threats
    - Counter-intelligence level reduces enemy success
    - Failed counter-intelligence: sabotage, assassination, information leaks
15. Add deterministic seeding: intelligence outcomes use `ISeededRng`
16. Wire into `GameBootstrap`: `SetupIntelligenceNetwork`, `TickIntelligenceNetwork`, `SaveIntelligenceNetwork`

## Main Task 2 — Implementation / Informants / Reports / Rumors / Operations / Counter-Intel / UI

1. Implement informant network:
   - Recruit informants (survivors, NPCs, faction contacts)
   - Informants have reliability, access, cover status
   - Informants gather intel in coverage area
   - Informants require payment (resources)
   - Informant cover can be compromised
   - Informant network logged
2. Implement intelligence reports:
   - Informants submit reports
   - Reports have type, subject, content, reliability
   - Reports can be verified (partially or fully)
   - Reports expire over time
   - Actionable reports trigger player decisions
   - Reports logged
3. Implement rumor system:
   - Rumors spread through factions/settlements
   - Rumors have credibility and spread level
   - Player can plant rumors (disinformation)
   - Player can counter enemy rumors
   - Rumors affect faction trust and morale
   - Rumor spread logged
4. Implement intelligence operations:
   - Player plans operations (surveillance, infiltration, etc.)
   - Operations require agents, time, resources
   - Success based on: agent skill + target difficulty + risk
   - Operations can succeed, fail, or be compromised
   - Operations logged
5. Implement counter-intelligence:
   - Enemy factions conduct intel operations against player
   - Counter-intelligence detects threats
   - Player can investigate and neutralize threats
   - Failed counter-intelligence: sabotage, leaks, assassination
   - Counter-intelligence logged
6. Implement intelligence quality:
   - Intelligence has reliability (source accuracy)
   - Intelligence has verification status
   - False intelligence can mislead player
   - Verification requires multiple sources
   - Intelligence quality logged
7. Implement intelligence UI:
   - Intelligence panel: informant network, reports, rumors
   - Informant detail: reliability, access, cover, payment
   - Report detail: type, subject, content, reliability, verification
   - Rumor panel: active rumors, spread, credibility
   - Operations panel: planned/active operations
   - Counter-intelligence panel: detected threats
   - Intelligence map: show informant coverage, faction intel
8. Create intelligence events:
    - "The Report" — intelligence report received
    - "The Informant" — new informant recruited
    - "The Rumor" — rumor spreading
    - "The Operation" — intelligence operation launched
    - "The Compromise" — informant cover blown
    - "The Counter" — counter-intelligence threat detected
    - "The Verification" — intelligence verified
    - "The Failure" — intelligence operation failed
9. Add intelligence quest hooks:
    - "The Spymaster" — build network of 10 informants
    - "The Analyst" — verify 20 intelligence reports
    - "The Puppet Master" — plant 5 successful rumors
    - "The Counter" — neutralize 3 enemy spy operations
    - "The Deep Cover" — maintain placed spy for 100 days
    - "The Intelligence Chief" — reach 90+ network reputation
    - "The Double Agent" — turn enemy informant to your side
10. Implement intelligence tutorial: first informant recruitment explains system
11. Add intelligence tooltips: hover over informant/report shows details
12. Create intelligence templates in data file (10+ informant types, 20+ report templates)
13. Implement intelligence persistence: network/reports/rumors saved with game state
14. Integrate with `FactionStanceEngine`: intelligence affects faction trust

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `FactionStanceEngine`: intelligence affects faction trust
2. Connect to `FactionBranchCoordinator`: intelligence reveals branch plans
3. Integrate with `HoldfastTradeSession`: trade contacts as informants
4. Connect to `SignalTriangulationSystem`: radio interception as intel source
5. Wire into `ExpeditionSystem`: field agents gather expedition intel
6. Connect to `CombatSystem`: military intelligence affects combat
7. Implement old-save compatibility: existing saves get no network, no reports
8. Add deterministic seeding: intelligence outcomes use `ISeededRng`
9. Create exploit prevention: intelligence is probabilistic, can't be gamed
10. Add tests: informant recruitment, report generation, rumor spreading, operations, counter-intelligence, save round-trip
11. Verify all informant types work correctly
12. Test edge cases: no network (current behavior), extensive network (intelligence flood)
13. Verify headless behavior: intelligence processes correctly without UI
14. Add data-integrity-selftest: intelligence validates against faction/location catalogs
15. Create `--intelligence-network-selftest` verb for CI validation

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --intelligence-network-selftest
```

## Risk

**LOW** — Intelligence network is straightforward with clear inputs (informants, operations) and outputs (reports, rumors). Risk of intelligence feeling like information overload. Mitigation: make intelligence quality variable (some reports false), show clear value (actionable intel), and ensure network management is strategic not tedious.

## Definition of Done

- `IntelligenceNetworkSystem.cs` exists with full `CaptureState/RestoreState`
- 5+ informant types (field agent, placed spy, trade contact, local source, defector)
- 7+ intelligence report types (faction movement, military strength, resource cache, leadership change, plot, terrain, trade)
- Rumor mechanics (spread, credibility, planting, countering)
- Intelligence operations (surveillance, interception, infiltration, extraction, recruitment)
- Counter-intelligence (detect and neutralize enemy intel threats)
- Intelligence quality tracking (reliability, verification)
- Intelligence events and quest hooks
- Save/load round-trip tested
- Deterministic intelligence outcomes verified
- Old saves load with no network, no reports
- Intelligence templates in data authority
- UI intelligence panel, informant detail, report detail, rumor panel, operations panel, counter-intelligence panel, intelligence map
- Cross-system integration (faction stance, branch coordinator, trade session, signal triangulation, expedition, combat)

## Follow-On Opportunities

- Intelligence specialization (survivors become expert spies/analysts)
- Intelligence legacy (famous operations remembered)
- Intelligence quests (specific intelligence goals)
- Intelligence events (major intelligence breakthrough, catastrophic failure)
- Intelligence trading (trade intelligence with other factions/settlements)
