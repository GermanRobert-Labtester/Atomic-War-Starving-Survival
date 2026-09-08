# Plan 139 — Combat → Faction Standing Bridge

## Goal

Connect combat outcomes to faction standing so that fighting faction-tagged combatants has political consequences. Killing a military patrol enrages the Garrison. Helping rebels in a firefight earns rebel favor. This transforms combat from isolated tactical encounters into politically meaningful actions that shape the faction landscape.

## Why

**Repository evidence:** `TacticalCombatSystem.cs` handles combat with `faction_id` on combatants (confirmed in `CombatCatalog.cs:601`). `FactionBranchCoordinator.cs:574` has `FactionStandingSummary` but it's never modified by combat outcomes. `FactionStanceEngine.cs` tracks per-faction trust (-100 to +100) but combat doesn't feed into it. The cross-system agent confirmed: "Combat outcomes do NOT modify `FactionBranchCoordinator` standing. Combatants have `faction_id` in the catalog but killing them does not trigger a standing delta."

**What is missing:** Players can fight faction-tagged enemies with zero political consequence. Killing military soldiers doesn't anger the military faction. Helping rebels doesn't earn rebel favor. Combat is politically meaningless — a missed opportunity for emergent storytelling and strategic depth.

**Why existing plans don't solve it:** Plan 45 (faction patrol encounters) adds patrol combat but not standing consequences. Plan 54 (combat catalog expansion) adds weapons/enemies but not political integration. Plan 63 (warlord doctrines) adds faction war mechanics but not standing from combat. Plan 92 (faction war dialogue) adds war content but not combat→standing bridge. No plan connects combat kills to faction standing.

**Player value:** Makes combat choices meaningful (who you fight matters), creates strategic depth (ally with one faction by fighting their enemies), generates emergent stories (a routine patrol fight escalates into faction war), and adds moral weight to violence (killing has consequences).

## Files / Systems to Inspect

- `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs` — combat mechanics
- `Assets/Ashfall.Core/Combat/CombatCatalog.cs` — combatant definitions with faction_id
- `Assets/Ashfall.Core/Factions/FactionBranchCoordinator.cs` — faction standing
- `Assets/Ashfall.Core/Economy/FactionStanceEngine.cs` — faction trust tracking
- `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` — expedition combat triggers
- `Assets/StreamingAssets/Data/combat_catalog.json` — combatant data
- NEW: `Assets/Ashfall.Core/Combat/CombatFactionBridge.cs`

## Main Task 1 — Foundation / System Contract

1. Create `CombatFactionBridge.cs` in `Assets/Ashfall.Core/Combat/`
2. Define `CombatFactionConsequence` DTO: `factionId`, `standingDelta` (-50 to +50), `reason` (killed_allied/killed_enemy/defeated_patrol/assisted_rebellion), `day`, `witnessed` bool, `severity` (minor/moderate/major)
3. Define `CombatFactionState` DTO: list of consequences, list of faction reactions, cooldown map (faction → last incident day)
4. Implement `CaptureState/RestoreState` with schema versioning
5. Define standing delta rules:
   - Killing allied faction combatant: -10 to -30 standing (depending on context)
   - Killing enemy faction combatant: +5 to +15 standing (with allied faction)
   - Defeating faction patrol: -15 to -25 standing (if faction is neutral/allied)
   - Assisting faction in combat: +10 to +20 standing
   - Witnessed incidents have 2x multiplier (reputation spreads)
   - Unwitnessed incidents have 0.5x multiplier
6. Create `ICombatFactionSink` interface for `TacticalCombatSystem` to report combat outcomes
7. Implement consequence calculation: read combatant faction_ids, compare to player's faction standing, calculate delta
8. Create cooldown system: same faction can't be incident more than once per 7 days (prevents farming)
9. Add deterministic seeding: witness detection uses `ISeededRng`
10. Wire into `GameBootstrap`: `SetupCombatFactionBridge`, `SaveCombatFaction`
11. Create `CombatFactionCatalogLoader` for standing delta rules per faction pair
12. Implement consequence logging: all combat→faction events recorded for UI/epilogue
13. Add faction reaction system: factions respond to combat incidents (diplomatic protests, bounties, praise)
14. Create UI hook: faction panel shows recent combat incidents and standing changes

## Main Task 2 — Implementation / Combat Integration / Faction Reactions

1. Implement combat outcome reporting:
   - `TacticalCombatSystem.OnCombatEnded` reports combatant faction_ids and outcomes
   - Bridge reads combatant data, determines which factions are involved
   - Bridge calculates standing deltas based on who killed whom
   - Bridge applies deltas to `FactionBranchCoordinator` and `FactionStanceEngine`
2. Create combat context detection:
   - Patrol encounter: faction patrol attacks player (defensive combat)
   - Raid assistance: player joins faction in attacking rival (offensive combat)
   - Rebellion support: player helps rebels against military (political combat)
   - Bandit confusion: player fights bandits who happen to have faction gear (no penalty)
   - Civilian crossfire: player kills faction combatant protecting civilians (complex morality)
3. Implement witness detection:
   - Witnesses determined by combat location (settlement = witnessed, wilderness = unwitnessed)
   - Faction radio intercepts can report incidents (witnessed even in wilderness)
   - Survivor companions always witness (if player faction is involved)
   - Witnesses amplify standing delta by 2x
4. Create faction reaction mechanics:
   - Diplomatic protest: faction sends envoy demanding explanation (dialogue choice)
   - Bounty: faction places bounty on player (hunter encounters)
   - Praise: faction sends reward (items, standing bonus)
   - Embargo: faction blocks trade (economic penalty)
   - Alliance offer: faction impressed, offers formal alliance (standing threshold)
5. Implement combat reputation system:
   - Player develops reputation as "faction killer" or "faction ally"
   - Reputation affects future encounter difficulty (more patrols if hostile)
   - Reputation affects trade options (hostile factions refuse trade)
   - Reputation affects quest availability (hostile factions block quests)
6. Create combat diplomacy options:
   - After combat, player can send envoy to explain (skill check)
   - Offer reparations (items, standing) to reduce penalty
   - Blame rival faction (deception check, may backfire)
   - Accept responsibility (reduced penalty, morale penalty)
7. Implement combat justification system:
   - Player can declare justification before combat (if faction is hostile)
   - Justified combat has 0.5x standing penalty
   - Unjustified combat has 1.5x standing penalty
   - Justification requires prior faction hostility or self-defense
8. Create combat faction events:
   - "The Patrol" — routine patrol turns violent, faction standing at risk
   - "The Rescue" — save faction convoy from ambush, standing bonus
   - "The Mistake" — kill allied faction by accident, damage control quest
   - "The Betrayal" — switch sides mid-combat, complex standing changes
   - "The Massacre" — kill many faction combatants, major standing penalty
9. Add UI: "Combat Record" panel showing faction combat history and standing impacts
10. Create combat journal: automatic log of politically significant combats
11. Implement combat faction tutorial: first-time combat with faction explains consequences
12. Add combat faction tooltips: hover over combatant shows faction and standing impact
13. Create 10 combat faction scenarios in data file
14. Add combat faction interaction with other systems:
    - `MoralChoiceSystem`: killing allies is moral choice
    - `ExpeditionSystem`: expedition combats affect faction standing
    - `ShelterDefenseSystem`: defending against faction attacks affects standing

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `TacticalCombatSystem`: combat outcomes reported to bridge
2. Connect to `FactionBranchCoordinator`: standing deltas applied
3. Integrate with `FactionStanceEngine`: trust values updated
4. Connect to `ExpeditionSystem`: expedition combats affect standing
5. Wire into `MoralChoiceSystem`: faction killing is moral choice
6. Connect to `ShelterDefenseSystem`: defense against factions affects standing
7. Implement old-save compatibility: existing saves get empty combat faction state
8. Add deterministic seeding: witness detection uses `ISeededRng`
9. Create exploit prevention: cooldowns prevent standing farming
10. Add tests: standing delta calculation, witness detection, cooldown, save round-trip, determinism
11. Verify catalog integrity: all faction IDs in combat catalog resolve
12. Test edge cases: no faction combatants (no consequence), all faction combatants (major consequence)
13. Verify headless behavior: bridge processes correctly without UI
14. Add data-integrity-selftest: combat faction rules validate against faction catalog
15. Create `--combat-faction-selftest` verb for CI validation

## State / System Interaction Model

```text
Combat encounter with faction-tagged combatants
├─ Combat resolves (kills, defeats, assists)
│  ├─ Bridge reads combatant faction_ids
│  ├─ Bridge determines context (patrol/raid/rebellion/bandit/crossfire)
│  ├─ Bridge detects witnesses (location, radio, companions)
│  ├─ Bridge calculates standing delta
│  │  ├─ Killing allied: -10 to -30 (witnessed = 2x)
│  │  ├─ Killing enemy: +5 to +15 (witnessed = 2x)
│  │  ├─ Defeating patrol: -15 to -25 (witnessed = 2x)
│  │  └─ Assisting faction: +10 to +20
│  ├─ Bridge applies delta to FactionBranchCoordinator
│  ├─ Bridge applies delta to FactionStanceEngine
│  └─ Bridge logs consequence
├─ Faction reacts
│  ├─ Diplomatic protest: envoy demands explanation
│  ├─ Bounty: hunter encounters triggered
│  ├─ Praise: reward sent, standing bonus
│  ├─ Embargo: trade blocked
│  └─ Alliance: formal alliance offered
├─ Player can respond
│  ├─ Send envoy: explain (skill check)
│  ├─ Offer reparations: items/standing to reduce penalty
│  ├─ Blame rival: deception check
│  └─ Accept responsibility: reduced penalty
└─ Reputation updates
   ├─ Future encounters affected (more patrols if hostile)
   ├─ Trade options affected (hostile factions refuse)
   └─ Quest availability affected (hostile factions block)
```

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --combat-faction-selftest
```

## Risk

**MEDIUM** — Combat→standing consequences can frustrate players if too severe or unpredictable. Risk of accidental faction hostility from misunderstood encounters. Mitigation: provide clear UI feedback (show faction before combat), allow justification/de-escalation, keep penalties moderate (max -30 per incident), and offer diplomatic recovery options.

## Definition of Done

- `CombatFactionBridge.cs` exists with full `CaptureState/RestoreState`
- Combat outcomes reported to bridge (kills, defeats, assists)
- Standing deltas calculated based on faction context
- Witness detection functional (location, radio, companions)
- Faction reactions implemented (protest, bounty, praise, embargo, alliance)
- Combat diplomacy options (envoy, reparations, blame, accept)
- Combat reputation system affects encounters/trade/quests
- Save/load round-trip tested
- Deterministic witness detection verified
- Old saves load without error
- 10 combat faction scenarios in data authority
- UI panel shows combat record and faction impacts
- Cross-system integration (combat, factions, expeditions, moral choice, shelter defense)

## Follow-On Opportunities

- Combat war crimes system (killing civilians, prisoners)
- Combat heroism system (saving allies, solo stands)
- Combat notoriety (famous battles remembered in epilogue)
- Combat mercenary work (fight for faction for pay)
- Combat training (faction teaches tactics based on standing)
