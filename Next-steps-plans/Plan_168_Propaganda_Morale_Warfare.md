# Plan 168 — Propaganda & Morale Warfare System

## Goal

Create a propaganda and morale warfare system where the shelter can create and distribute propaganda, broadcast messages, conduct psychological operations, and influence faction morale and civilian allegiance. Currently `PaperPrintingCatalog.cs` has a `StencilPropagandaSmearEntry` data type for narrative propaganda smear logs, and `VerdictRadioSystem.cs` handles scripted faction broadcasts, but there is no player-driven propaganda system — no mechanic for creating propaganda, no influence on faction morale, no psychological operations as gameplay. This plan adds an information warfare layer to faction interactions.

## Why

**Repository evidence:** Grep for `propaganda`, `PsyOps`, `morale_warfare`, `broadcast_system` in Core returns only `StencilPropagandaSmearEntry` in `PaperPrintingCatalog.cs` (narrative data class for stencil propaganda logs — not a gameplay system). `VerdictRadioSystem.cs` and `VerdictCensusBroadcast.cs` handle scripted faction broadcasts but are narrative events, not player-driven. `PsychologicalContaminationSystem.cs` (233 lines) handles trauma FROM locations (thousand-yard stare, disgust cascade) — not psychological warfare AS A TOOL. `ClandestineInsurgency` appears only as a panel registry name with no backing system.

**What is missing:** No player-driven propaganda system. No mechanic for creating/distributing propaganda materials. No influence on faction morale through information warfare. No broadcast capability (player creating radio content). No leaflet drops, no wall postings, no rumor campaigns. The `StencilPropagandaSmearEntry` data structure exists but has no gameplay system consuming it.

**Why existing plans don't solve it:** Plan 131 (rumor network) adds rumor spreading between survivors but not faction-level propaganda. Plan 139 (combat→faction) connects combat to faction relations but not information warfare. Plan 153 (espionage) adds covert operations but not propaganda/psyops. Plan 157 (communications) adds communication infrastructure but not propaganda content creation. No plan addresses propaganda as a gameplay tool.

**Player value:** Creates strategic depth (information warfare alongside military/economic), adds moral complexity (propaganda can be truthful or deceptive), provides non-violent faction interaction (win hearts and minds), and generates emergent stories (propaganda discovered, broadcast intercepted, psyops backfire).

## Files / Systems to Inspect

- `Assets/Ashfall.Core/Narrative/PaperPrintingCatalog.cs` — existing propaganda data type
- `Assets/Ashfall.Core/Verdict/VerdictRadioSystem.cs` — faction broadcast system
- `Assets/Ashfall.Core/Factions/FactionBranchCoordinator.cs` — faction interactions
- `Assets/Ashfall.Core/Factions/FactionStanceEngine.cs` — faction trust/stance
- `Assets/Ashfall.Core/MoralChoice/` — moral choice system
- NEW: `Assets/Ashfall.Core/Propaganda/PropagandaSystem.cs`
- NEW: `Assets/StreamingAssets/Data/propaganda_templates.json`

## Main Task 1 — Foundation / System Contract

1. Create `PropagandaSystem.cs` in `Assets/Ashfall.Core/Propaganda/`
2. Define `PropagandaType` DTO: `typeId`, `typeName` (leaflet/radio_broadcast/wall_posting/rumor_campaign/broadcast_intercept), `creationCost` (list of resources), `distributionMethod` (how it reaches target), `effectiveness` (base influence modifier), `detectionRisk` (0-1, chance of being discovered), `moralCost` (reputation penalty if caught lying)
3. Define `PropagandaMessage` DTO: `messageId`, `messageType` (truth/half_truth/lie/exaggeration), `targetFaction` (faction ID), `targetAudience` (civilians/military/leadership), `theme` (hope/fear/unity/division/triumph/sacrifice), `content` (text), `quality` (0-100, affects effectiveness), `authorId` (survivor ID)
4. Define `PropagandaCampaign` DTO: `campaignId`, `campaignName`, `objective` (undermine_faction/boost_morale/recruit_defectors/protect_identity/destabilize_region), `targetFaction`, `messages` (list of propaganda message IDs), `duration` (days), `status` (planned/active/complete/compromised), `effectiveness` (0-100), `detected` bool
5. Define `PropagandaState` DTO: list of created messages, list of active/completed campaigns, list of discovered templates, shelter propaganda reputation (known for truth/deception), faction morale effects from propaganda
6. Implement `CaptureState/RestoreState` with schema versioning
7. Define propaganda types:
   - **Leaflets**: printed materials distributed physically, low cost, low reach, moderate effectiveness
   - **Radio broadcasts**: audio messages broadcast on radio frequencies, moderate cost, high reach, high effectiveness
   - **Wall postings**: messages posted on walls in faction territory, low cost, very low reach, low effectiveness
   - **Rumor campaigns**: spread rumors through trade/travel networks, low cost, slow spread, variable effectiveness
   - **Broadcast intercepts**: hijack enemy radio frequencies, high cost, high risk, very high effectiveness
8. Define message truthfulness:
   - **Truth**: factual content, lower effectiveness but no moral cost, builds credibility
   - **Half-truth**: partially factual, moderate effectiveness, low moral cost
   - **Lie**: fabricated content, high effectiveness, high moral cost if discovered
   - **Exaggeration**: truth amplified, moderate-high effectiveness, moderate moral cost
9. Define propaganda themes:
   - **Hope**: inspire optimism, boost allied morale
   - **Fear**: instill fear, undermine enemy morale
   - **Unity**: promote cooperation, strengthen alliances
   - **Division**: create dissent, weaken enemy cohesion
   - **Triumph**: celebrate victories, boost confidence
   - **Sacrifice**: honor losses, strengthen resolve
10. Define propaganda effectiveness:
    - Quality of writing (survivor skill)
    - Truthfulness (truth less effective but safer)
    - Distribution method (radio > leaflet > wall posting)
    - Target audience receptiveness (war-weary = more receptive to hope)
    - Faction counter-propaganda (reduces effectiveness)
    - Shelter credibility (history of truth/lie affects trust)
11. Define detection mechanics:
    - Each propaganda type has detection risk
    - Detection means target faction knows shelter is conducting propaganda
    - Detected propaganda: faction hostility increases, shelter reputation damaged
    - Lie detection: severe reputation damage ("liars")
    - Truth detection: mild reaction ("they're broadcasting against us")
12. Add deterministic seeding: propaganda outcomes use `ISeededRng`
13. Wire into `GameBootstrap`: `SetupPropaganda`, `TickPropaganda`, `SavePropaganda`
14. Create `PropagandaTemplateCatalogLoader` for message templates
15. Implement propaganda UI: campaign manager panel

## Main Task 2 — Implementation / Creation / Distribution / Campaigns / Counter

1. Implement propaganda creation:
   - Player selects propaganda type (leaflet, radio, wall, rumor, intercept)
   - Player selects message truthfulness (truth, half-truth, lie, exaggeration)
   - Player selects theme (hope, fear, unity, division, triumph, sacrifice)
   - Player selects target faction and audience
   - Survivor with writing skill creates message (quality based on skill)
   - Resources consumed (paper, ink, radio equipment, etc.)
   - Message stored in propaganda state
2. Implement propaganda distribution:
   - Leaflets: require survivor to distribute (expedition to faction territory)
   - Radio: require radio equipment + broadcasting survivor
   - Wall postings: require survivor to post (expedition, high detection risk)
   - Rumors: spread through trade network (slow, variable)
   - Broadcast intercepts: require captured enemy radio equipment
   - Distribution triggers effectiveness calculation
3. Implement propaganda campaigns:
   - Campaign = series of coordinated propaganda messages
   - Campaign has objective (undermine, boost, recruit, protect, destabilize)
   - Campaign runs for specified duration
   - Campaign effectiveness accumulates over time
   - Campaign can be compromised (detection triggers compromise)
   - Compromised campaign: effectiveness drops, reputation damage
4. Implement faction morale effects:
    - Propaganda affects target faction morale
    - Low faction morale: reduced recruitment, lower trade prices, more defectors
    - High faction morale: increased aggression, better trade terms, more allies
    - Propaganda can shift faction stance (neutral → friendly, friendly → hostile)
    - Effects are gradual (propaganda is slow warfare)
5. Implement counter-propaganda:
    - Factions conduct counter-propaganda against player
    - Counter-propaganda reduces effectiveness of player propaganda
    - Faction radio broadcasts undermine shelter credibility
    - Player can counter-counter-propaganda (propaganda war)
    - Counter-propaganda detected as propaganda event
6. Implement propaganda consequences:
    - Successful propaganda: faction morale affected, objectives met
    - Failed propaganda: resources wasted, no effect
    - Detected propaganda: faction hostility, reputation damage
    - Lie exposed: severe reputation damage ("liars"), credibility loss
    - Truth acknowledged: mild credibility boost
    - Propaganda affects refugee flow (good propaganda → more refugees)
7. Implement propaganda moral choices:
    - Truth vs. lie: truth safer but less effective
    - Target civilians vs. military: civilians more affected, military more angry
    - Undermine allies: possible but reputation-costly
    - Propaganda about own losses: can boost resolve or undermine morale
    - Each choice has moral weight tracked by moral choice system
8. Create propaganda events:
   - "The Broadcast" — radio propaganda transmitted
   - "The Leaflet" — leaflets distributed in faction territory
   - "The Wall" — message posted on faction wall
   - "The Rumor" — rumor campaign spreads
   - "The Intercept" — enemy broadcast hijacked
   - "The Discovery" — propaganda discovered by target faction
   - "The Backfire" — propaganda campaign fails spectacularly
   - "The Defector" — faction member defects due to propaganda
9. Add propaganda quest hooks:
   - "The Propagandist" — create first propaganda message
   - "The Campaign" — complete propaganda campaign
   - "The Voice" — broadcast radio propaganda
   - "The Underground" — distribute leaflets in enemy territory
   - "The War of Words" — win propaganda war against faction
   - "The Truth" — run all-truth propaganda campaign
   - "The Defector" — cause faction member to defect via propaganda
10. Implement propaganda UI:
    - Campaign manager: create, manage, view campaigns
    - Message editor: create propaganda messages
    - Distribution planner: plan distribution methods
    - Effectiveness tracker: view campaign progress
    - Faction morale display: see propaganda effects
11. Add propaganda journal: automatic log of propaganda events
12. Implement propaganda tutorial: first propaganda creation explains system
13. Add propaganda tooltips: hover over campaign shows effectiveness
14. Create 15 propaganda templates in data file

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `FactionBranchCoordinator`: propaganda affects faction morale
2. Connect to `FactionStanceEngine`: propaganda shifts faction stance
3. Integrate with `PaperPrintingCatalog`: leaflets use printing system
4. Connect to `VerdictRadioSystem`: radio propaganda uses radio infrastructure
5. Wire into `MoralChoice` system: propaganda truthfulness tracked morally
6. Connect to `NarrativeSystem`: propaganda events generate narrative
7. Implement old-save compatibility: existing saves get empty propaganda state
8. Add deterministic seeding: propaganda outcomes use `ISeededRng`
9. Create exploit prevention: propaganda costs resources, detection has consequences
10. Add tests: message creation, distribution, campaign mechanics, detection, save round-trip
11. Verify catalog integrity: all template/faction IDs resolve
12. Test edge cases: no propaganda (no effects), extensive propaganda (complex campaigns)
13. Verify headless behavior: propaganda processes correctly without UI
14. Add data-integrity-selftest: propaganda templates validate against faction catalogs
15. Create `--propaganda-selftest` verb for CI validation

## State / System Interaction Model

```text
Propaganda & morale warfare
├─ Propaganda creation
│  ├─ Type: leaflet/radio/wall/rumor/intercept
│  ├─ Truthfulness: truth/half-truth/lie/exaggeration
│  ├─ Theme: hope/fear/unity/division/triumph/sacrifice
│  ├─ Target: faction + audience
│  ├─ Quality: survivor writing skill
│  └─ Resources consumed
├─ Propaganda distribution
│  ├─ Leaflets: physical distribution (expedition)
│  ├─ Radio: broadcast via radio equipment
│  ├─ Wall postings: physical posting (high risk)
│  ├─ Rumors: spread through trade network
│  ├─ Intercepts: hijack enemy frequencies
│  └─ Distribution triggers effectiveness calc
├─ Propaganda campaigns
│  ├─ Coordinated message series
│  ├─ Objectives: undermine/boost/recruit/protect/destabilize
│  ├─ Duration-based effectiveness
│  ├─ Can be compromised (detection)
│  └─ Effects accumulate over time
├─ Faction morale effects
│  ├─ Propaganda affects target faction morale
│  ├─ Low morale: reduced recruitment, trade, defectors
│  ├─ High morale: aggression, trade, allies
│  ├─ Gradual stance shifts
│  └─ Affects refugee flow
├─ Detection & consequences
│  ├─ Detection risk per type
│  ├─ Detected: faction hostility, reputation damage
│  ├─ Lie exposed: severe damage
│  ├─ Truth detected: mild reaction
│  └─ Moral choices tracked
└─ Integration
   ├─ Factions (morale, stance)
   ├─ Printing (leaflets)
   ├─ Radio (broadcasts)
   ├─ Moral choice (truth tracking)
   ├─ Narrative (propaganda events)
   └─ Trade (rumor spreading)
```

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --propaganda-selftest
```

## Risk

**MEDIUM** — Propaganda system complexity can overwhelm if too many campaign options and moral choices exist. Risk of propaganda feeling like a spreadsheet exercise rather than meaningful warfare. Mitigation: keep campaign options clear, show effectiveness feedback, make detection consequences meaningful, and integrate with moral choice system for weight.

## Definition of Done

- `PropagandaSystem.cs` exists with full `CaptureState/RestoreState`
- 5 propaganda types implemented (leaflet, radio, wall, rumor, intercept)
- 4 truthfulness levels (truth, half-truth, lie, exaggeration)
- 6 propaganda themes (hope, fear, unity, division, triumph, sacrifice)
- Propaganda campaign system with objectives and duration
- Faction morale effects from propaganda
- Detection and consequence mechanics
- Counter-propaganda from factions
- Moral choice integration (truthfulness tracked)
- Propaganda events and quest hooks
- Save/load round-trip tested
- Deterministic propaganda outcomes verified
- Old saves load without error
- 15 propaganda templates in data authority
- UI campaign manager panel
- Cross-system integration (factions, printing, radio, moral choice, narrative, trade)

## Follow-On Opportunities

- Propaganda art (visual propaganda creation)
- Propaganda legacy (famous propaganda campaigns remembered)
- Propaganda quests (specific propaganda objectives)
- Propaganda competitions (faction propaganda wars)
- Propaganda archives (collection of all propaganda created)
