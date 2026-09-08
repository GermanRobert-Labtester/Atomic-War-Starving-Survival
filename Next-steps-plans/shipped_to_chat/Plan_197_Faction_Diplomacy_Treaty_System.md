# Plan 197 — Faction Diplomacy & Treaty System

## Goal

Create a faction diplomacy and treaty system where players can negotiate formal agreements (non-aggression pacts, trade alliances, mutual defense, intelligence sharing) with factions, manage diplomatic relations through envoys and negotiations, and build a network of alliances that shape the geopolitical landscape. Currently `FactionStanceEngine.cs` tracks per-faction trust and trade stances, `FactionBranchCoordinator.cs` coordinates faction branches with standing/trust, and `HoldfastTradeSession.cs` handles faction-gated trade — but there is no formal diplomacy system, no treaty negotiation, no alliance formation, no diplomatic missions, no treaty enforcement. Faction relations are reduced to a single "trust" number. This plan adds geopolitical depth to faction interactions.

## Why

**Repository evidence:** Grep for `DiplomacySystem`, `TreatyNegotiation`, `AllianceSystem`, `DiplomaticRelation`, `NegotiateTreaty`, `FormAlliance` in Core returns ZERO matches. `FactionStanceEngine.cs` tracks trust (-100 to +100) and trade stances (HostileRaid/Rob/Refuse/Trade/ShareIntel). `FactionBranchCoordinator.cs` (661 lines) coordinates Military/Rebel/Independent branches with standing, trust, point-of-no-return. `HoldfastTradeSession.cs` (682 lines) handles faction-gated trade. Plan 139 (Combat Faction Standing Bridge) mentions "Alliance offer: faction impressed, offers formal alliance" as a combat consequence. Plan 138 (Shelter Defense) mentions "Faction envoys: diplomatic visits." Plan 160 (Expedition Colony) mentions "form alliance with faction near colony." But no dedicated diplomacy/treaty system exists.

**What is missing:** No formal diplomacy system. No treaty negotiation mechanics. No alliance formation. No diplomatic missions (envoys, negotiations). No treaty types (non-aggression, trade, defense, intelligence). No treaty enforcement. No diplomatic reputation. No treaty breaking consequences. Faction relations are a single trust number, not a rich diplomatic landscape.

**Why existing plans don't solve it:** Plan 139 (combat standing bridge) adds alliance offers as combat consequences but not diplomacy system. Plan 138 (shelter defense) mentions diplomatic visits but not treaty mechanics. Plan 160 (expedition colony) mentions forming alliances but not diplomacy system. Plan 131 (information network) mentions propaganda campaigns but not diplomacy. No plan addresses faction diplomacy as a system.

**Player value:** Creates strategic depth (build alliances, manage relations), adds geopolitical gameplay (negotiate treaties, honor agreements), generates emergent stories (betrayed alliances, diplomatic crises), and makes faction interactions more meaningful than just "increase trust number."

## Files / Systems to Inspect

- `Assets/Ashfall.Core/Factions/FactionStanceEngine.cs` — faction trust/stances
- `Assets/Ashfall.Core/Factions/FactionBranchCoordinator.cs` — faction branches
- `Assets/Ashfall.Core/HoldfastTradeSession.cs` — faction-gated trade
- `Assets/Ashfall.Core/RegionalTreatySystem.cs` — regional treaties
- NEW: `Assets/Ashfall.Core/Diplomacy/FactionDiplomacySystem.cs`
- NEW: `Assets/StreamingAssets/Data/treaty_templates.json`

## Main Task 1 — Foundation / System Contract

1. Create `FactionDiplomacySystem.cs` in `Assets/Ashfall.Core/Diplomacy/`
2. Define `DiplomaticRelation` DTO: `relationId`, `factionId`, `relationLevel` (hostile/unfriendly/neutral/friendly/allied), `diplomaticReputation` (0-100, how trustworthy player is), `treatyCount` (active treaties), `lastDiplomaticAction` (day), `envoyAssigned` (survivor_id or null), `relationTrend` (improving/stable/declining)
3. Define `Treaty` DTO: `treatyId`, `treatyType` (non_aggression/trade_alliance/mutual_defense/intelligence_sharing/tribute/vasallage), `factionA` (player shelter), `factionB` (faction_id), `signedDay`, `durationDays` (-1 = permanent), `terms` (list of treaty terms), `benefits` (list of benefits for each side), `obligations` (list of obligations for each side), `status` (active/expired/violated/renounced), `signatories` (list of survivor_ids who negotiated)
4. Define `TreatyTerm` DTO: `termId`, `termType` (resource_exchange/military_support/intelligence_access/territory_access/trade_discount/tribute_payment), `description`, `value` (quantity/frequency), `condition` (when term applies)
5. Define `DiplomaticMission` DTO: `missionId`, `missionType` (negotiate_treaty/renew_treaty/deliver_tribute/request_aid/propose_alliance/deliver_message), `targetFactionId`, `assignedEnvoyId`, `missionDay`, `durationDays`, `successChance` (0-100), `status` (planned/en_route/in_progress/completed/failed)
6. Define `TreatyViolation` DTO: `violationId`, `treatyId`, `violationType` (attacked_ally/failed_tribute/refused_aid/broke_embargo/terminated_treaty), `violationDay`, `violator` (faction_id or player), `consequences` (list of penalties), `diplomaticReputationLoss` (0-100)
7. Define `FactionDiplomacyState` DTO: list of diplomatic relations, list of active treaties, list of diplomatic missions, list of treaty violations, diplomatic reputation (global), diplomacy settings (auto-renew treaties bool, treaty notifications bool)
8. Implement `CaptureState/RestoreState` with schema versioning
9. Define treaty types (6+ types):
   - **Non-Aggression Pact**: neither side attacks the other, duration 30-90 days
   - **Trade Alliance**: preferential trade terms, reduced tariffs, exclusive access, duration 60-180 days
   - **Mutual Defense**: if one is attacked, other comes to aid, duration 90-365 days
   - **Intelligence Sharing**: share intel on faction movements, quest hints, duration 30-120 days
   - **Tribute Agreement**: player pays regular tribute, faction provides protection, duration 60-180 days
   - **Vassalage**: player becomes faction vassal, faction provides protection/resources, player pays tribute/follows orders, duration 180-365 days
10. Define diplomatic relation levels:
    - **Hostile** (-100 to -50): faction attacks on sight, no trade, no diplomacy
    - **Unfriendly** (-49 to -10): faction refuses trade, diplomacy difficult
    - **Neutral** (-9 to +9): basic trade available, diplomacy possible
    - **Friendly** (+10 to +49): preferential trade, diplomacy easier, may offer treaties
    - **Allied** (+50 to +100): full alliance, mutual benefits, joint operations
    - Relation level affects treaty availability and negotiation difficulty
11. Define diplomatic reputation:
    - Reputation tracks how trustworthy player is (0-100)
    - Honoring treaties: +reputation
    - Breaking treaties: -reputation (severe penalty)
    - Successful negotiations: +reputation
    - Failed missions: -reputation
    - High reputation: easier negotiations, better treaty terms
    - Low reputation: factions distrust player, harder to negotiate
12. Define diplomatic missions:
    - Player assigns envoy survivor to diplomatic mission
    - Mission type determines objective (negotiate treaty, deliver tribute, etc.)
    - Mission takes time (travel + negotiation days)
    - Success chance based on: envoy skill + faction relation + diplomatic reputation
    - Successful mission: treaty signed/mission completed
    - Failed mission: treaty rejected, reputation loss
13. Define treaty negotiation:
    - Player proposes treaty terms
    - Faction evaluates terms based on: relation level, reputation, strategic value
    - Faction may accept, reject, or counter-propose
    - Negotiation takes time (multiple rounds possible)
    - Treaty signed when both sides agree
14. Define treaty enforcement:
    - Treaties have obligations (tribute payments, military aid, etc.)
    - Obligations tracked daily
    - Failed obligations: treaty violation
    - Treaty violations: reputation loss, faction hostility, possible war
    - Treaties can be voluntarily renounced (reputation loss)
15. Add deterministic seeding: diplomatic outcomes use `ISeededRng`
16. Wire into `GameBootstrap`: `SetupFactionDiplomacy`, `TickFactionDiplomacy`, `SaveFactionDiplomacy`

## Main Task 2 — Implementation / Relations / Treaties / Missions / Negotiations / UI

1. Implement diplomatic relations:
   - Each faction has diplomatic relation level
   - Relation affects treaty availability
   - Relation changes based on actions (trade, combat, treaties)
   - Relation displayed in faction panel
2. Implement treaty system:
   - Player can propose treaties to factions
   - Treaty terms defined (benefits, obligations)
   - Faction evaluates treaty proposal
   - Treaty negotiation (accept/reject/counter)
   - Treaty signed and active
   - Treaty obligations tracked
   - Treaty expiration/renewal
3. Implement diplomatic missions:
   - Player assigns envoy to mission
   - Mission type (negotiate, deliver, request)
   - Mission duration (travel + negotiation)
   - Success check (envoy skill + relation + reputation)
   - Mission outcome (success/failure)
   - Mission logged
4. Implement treaty negotiation:
   - Player proposes treaty terms
   - Faction evaluates (relation, reputation, strategic value)
   - Negotiation rounds (accept/reject/counter)
   - Treaty signed when agreed
   - Negotiation logged
5. Implement treaty enforcement:
   - Track treaty obligations daily
   - Tribute payments due on schedule
   - Military aid required when ally attacked
   - Failed obligations: treaty violation
   - Violation consequences (reputation loss, hostility)
   - Voluntary renunciation (reputation loss)
6. Implement diplomatic reputation:
   - Track global diplomatic reputation (0-100)
   - Reputation affects negotiation difficulty
   - Reputation affects treaty terms
   - Reputation changes from actions
   - Reputation displayed in UI
7. Implement treaty benefits:
   - Non-aggression: faction doesn't attack
   - Trade alliance: preferential trade terms
   - Mutual defense: faction provides military aid
   - Intelligence sharing: faction provides intel
   - Tribute: faction provides protection
   - Vassalage: faction provides resources/protection
8. Implement treaty UI:
   - Diplomacy panel: all faction relations
   - Treaty panel: active treaties, obligations, expiration
   - Negotiation panel: propose terms, negotiate
   - Mission panel: assign envoys, track missions
   - Reputation display: diplomatic reputation
   - Treaty log: history of treaties/violations
9. Implement diplomatic events:
    - "The Treaty" — treaty signed
    - "The Mission" — diplomatic mission dispatched
    - "The Negotiation" — treaty negotiation started
    - "The Alliance" — alliance formed
    - "The Violation" — treaty violated
    - "The Renunciation" — treaty renounced
    - "The Betrayal" — faction breaks treaty
    - "The Crisis" — diplomatic crisis (multiple violations)
10. Add diplomatic quest hooks:
    - "The Diplomat" — sign 5 treaties
    - "The Alliance" — form alliance with 3 factions
    - "The Peacemaker" — maintain non-aggression with 5 factions for 100 days
    - "The Envoy" — complete 10 diplomatic missions
    - "The Reputation" — reach 90+ diplomatic reputation
    - "The Network" — have active treaties with 8 factions
    - "The Honor" — never violate a treaty in campaign
11. Implement diplomatic tutorial: first treaty negotiation explains system
12. Add diplomatic tooltips: hover over faction shows relation, treaties
13. Create treaty templates in data file (6+ treaty types)
14. Implement diplomatic persistence: treaties/relations saved with game state
15. Integrate with `FactionStanceEngine`: diplomatic relations affect trust

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `FactionStanceEngine`: diplomatic relations affect trust
2. Connect to `FactionBranchCoordinator`: treaties affect branch decisions
3. Integrate with `HoldfastTradeSession`: trade treaties affect trade terms
4. Connect to `RegionalTreatySystem`: regional treaties integrate with diplomacy
5. Wire into `CombatSystem`: non-aggression prevents combat
6. Connect to `ExpeditionSystem`: alliances affect expedition safety
7. Implement old-save compatibility: existing saves get neutral relations, no treaties
8. Add deterministic seeding: diplomatic outcomes use `ISeededRng`
9. Create exploit prevention: treaties have real obligations, can't be gamed
10. Add tests: diplomatic relations, treaty negotiation, missions, enforcement, violations, save round-trip
11. Verify all treaty types work correctly
12. Test edge cases: no treaties (current behavior), many treaties (complex network)
13. Verify headless behavior: diplomacy processes correctly without UI
14. Add data-integrity-selftest: treaties validate against faction catalogs
15. Create `--faction-diplomacy-selftest` verb for CI validation

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --faction-diplomacy-selftest
```

## Risk

**LOW** — Faction diplomacy is straightforward with clear inputs (negotiations, treaties) and outputs (agreements, violations). Risk of diplomacy feeling like spreadsheet management. Mitigation: make negotiations meaningful, show clear benefits/consequences, allow auto-renewal, and ensure treaties feel impactful not bureaucratic.

## Definition of Done

- `FactionDiplomacySystem.cs` exists with full `CaptureState/RestoreState`
- 6+ treaty types (non-aggression, trade alliance, mutual defense, intelligence sharing, tribute, vassalage)
- 5 diplomatic relation levels (hostile, unfriendly, neutral, friendly, allied)
- Diplomatic reputation system (0-100)
- Diplomatic missions (negotiate, deliver, request)
- Treaty negotiation mechanics (propose, evaluate, accept/reject/counter)
- Treaty enforcement (obligations, violations, consequences)
- Treaty benefits (protection, trade, intel, resources)
- Diplomatic events and quest hooks
- Save/load round-trip tested
- Deterministic diplomatic outcomes verified
- Old saves load with neutral relations, no treaties
- Treaty templates in data authority
- UI diplomacy panel, treaty panel, negotiation panel, mission panel
- Cross-system integration (faction stance, branch coordinator, trade session, regional treaty, combat, expedition)

## Follow-On Opportunities

- Diplomacy specialization (survivors become expert diplomats)
- Diplomacy legacy (famous treaties remembered)
- Diplomacy quests (specific diplomatic goals)
- Diplomacy events (diplomatic summits, international crises)
- Diplomacy trading (trade diplomatic favors between factions)
