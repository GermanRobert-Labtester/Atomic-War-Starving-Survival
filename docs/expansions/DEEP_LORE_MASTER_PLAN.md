# ASHFALL — DEEP LORE & CHARACTER PROGRESSION: IMPLEMENTATION PLAN

> **Status**: Planning complete — ~90% data-driven, ~10% new C#
> **Target**: Rich narrative worldbuilding without heavy new system code
> **Owner**: Pi (C#, survivor entries, event data, narrative JSON) + Cursor (lore UI, faction map)

---

## I. EXISTING INFRASTRUCTURE AUDIT — What We Reuse

This expansion is **primarily data, not code**. The project already has all the narrative plumbing:

| Existing System | File | What It Does | How We Use It |
|----------------|------|-------------|---------------|
| `PersonalQuestSystem` | `Survivors/PersonalQuestSystem.cs` | 70+ latent expert traits, milestone tracking, quest activation on day 30 or morale recovery | Add 4 new questlines for named characters |
| `QuestlineSO` | `Survivors/QuestlineSO.cs` | Quest definition with stages, spawn nodes, narrative events | Create 4 new SO assets for Aris/Maya/Victor/Elena |
| `SurvivorArchetypeSO` | `Data/SurvivorArchetypeSO.cs` | Archetype definition with bio, traits, personal quest | Add 4 new archetypes |
| `CharacterStorySystem` | `Core/CharacterStorySystem.cs` | 3 character arcs (Reporter, Plumber, Defector) with story stages | Pattern reference — our 4 new arcs follow same structure |
| `EventRunner` + `GameEvent` | `Events/EventRunner.cs`, `Events/GameEvent.cs` | Narrative events with choices, effects, trust deltas | Add branch-point events for each character |
| `MoralBranchingSystem` | `Survivors/MoralBranchingSystem.cs` | Tracks empathy vs pragmatism axis | Already tracks this — we extend it |
| `BeliefSystem` | `Survivors/BeliefSystem.cs` | Survivor belief profiles, risk perception | Already has worldview tracking |
| `FactionLoreVoiceLines` | `Data/FactionLoreVoiceLines.cs` | Faction-specific dialogue and lore | Extend with new faction lore entries |
| `MoralChronicleBridge` | `Core/MoralChronicleBridge.cs` | Endgame narrative generation | Reads survivor arcs for chronicle |
| `SurvivorDiariesSystem` | `Survivors/SurvivorDiariesSystem.cs` | Personal journal entries | Auto-generates milestone entries |
| `JournalSystem` | `Events/JournalSystem.cs` | Diegetic journal with knowledge entries | Records world history discoveries |
| `BunkerMicroNarrativeSystem` | `Events/BunkerMicroNarrativeSystem.cs` | Small story moments | Triggers character-specific micro-narratives |
| `Survivor.cs` | `Survivors/Survivor.cs` | All survivor state | Add `NarrativeArcMilestone` (int), `EmpathyNumbnessRating` already exists via `MoralBranchDirection` |

**Net new C#**: 1 system (`SurvivorNarrativeArcSystem`), ~100 lines
**Net new data**: 4 survivor entries, 4 questlines, ~15 narrative events, world history JSON, faction lore JSON
**Net new UI**: 3 widgets for Cursor

---

## II. WHAT WE BUILD — Phase Breakdown

### Phase L1: World History Timeline (Data-Only)

**File**: `Assets/StreamingAssets/Data/world_history.json`

A JSON timeline of key events the player discovers through journals, radio intercepts, and location exploration. Each entry has:
- `era`: "pre_exchange", "hour_zero", "black_sky", "ashfall"
- `year_month`: e.g. "Exchange+3M", "Exchange+2Y"
- `title`: Short headline
- `body`: 2-4 sentences of cold, exhausted narrative
- `discovery_location_id`: Where this lore is found
- `discovery_trigger`: "journal", "radio", "location_explore", "survivor_dialogue"
- `knowledge_key`: snake_case id for the JournalSystem

**Example entry**:
```json
{
  "era": "hour_zero",
  "year_month": "Exchange+0",
  "title": "The Forty-Five Minute War",
  "body": "Atmospheric detonations at 40,000 feet triggered continent-wide EMP waves. Groundburst strikes followed — silos, dams, industrial hubs. The exchange lasted less time than it takes to cook a meal. When the sirens stopped, 80% of the northern hemisphere's electronics were dead and 200 million tons of irradiated particulate matter were rising into the stratosphere.",
  "discovery_location_id": "loc_comm_array",
  "discovery_trigger": "radio_intercept",
  "knowledge_key": "lore_hour_zero_duration"
}
```

**Total entries**: ~25, spanning all 4 eras. 6-7 per era.

---

### Phase L2: Four Named Survivor Entries (Data + JSON)

Add 4 survivors to `Assets/StreamingAssets/Data/survivors.json`:

#### Dr. Aris Thorne — The Shattered Engineer
```json
{
  "id": "aris_thorne",
  "displayName": "Dr. Aris Thorne",
  "profession": "Chief Structural Engineer",
  "bio": "Designed the civilian shelter network before the war. Watched his wife and daughter die when an unreinforced hatch failed under an ash slide. Every crack in the concrete feels personal.",
  "baseHealth": 95,
  "phantom_background_id": "machinist",
  "pre_war_profession_id": "machinist",
  "belief_profile_id": "atheist_rationalist",
  "personal_keepsake_item_id": "blueprint_roll",
  "latentExpertTrait": "trait_resilient_builder",
  "activeQuestlineId": "quest_the_cracked_floor",
  "baseTraits": ["trait_methodical", "trait_insomniac", "trait_guilt_ridden"]
}
```

#### Maya Lin — The Signalist
```json
{
  "id": "maya_lin",
  "displayName": "Maya Lin",
  "profession": "Radio Signal Technician",
  "bio": "Spent three years locked in a radio room scanning static for her brother's unit. She speaks in calm, measured cadences over the airwaves but flinches at loud noises and struggles with the cold reality of life underground.",
  "baseHealth": 80,
  "phantom_background_id": "generic",
  "pre_war_profession_id": "",
  "belief_profile_id": "collectivist_solidarity",
  "personal_keepsake_item_id": "radio_headset",
  "latentExpertTrait": "trait_voice_of_hope",
  "activeQuestlineId": "quest_the_dying_signal",
  "baseTraits": ["trait_keen_hearing", "trait_denial", "trait_fragile_health"]
}
```

#### Captain Victor Vance — The Garrison Deserter
```json
{
  "id": "victor_vance",
  "displayName": "Captain Victor Vance",
  "profession": "Garrison Tactical Commander",
  "bio": "Deserted the Iron Garrison after refusing an order to execute civilians who failed a food tribute quota. Views survival through a strict tactical lens. His presence unnerves the civilian survivors who see only the uniform.",
  "baseHealth": 110,
  "phantom_background_id": "former_soldier",
  "pre_war_profession_id": "",
  "belief_profile_id": "military_discipline",
  "personal_keepsake_item_id": "service_pistol",
  "latentExpertTrait": "trait_guardian_captain",
  "activeQuestlineId": "quest_the_refugee_mass_influx",
  "baseTraits": ["trait_martial_discipline", "trait_unforgiving", "trait_tactical_mind"]
}
```

#### Dr. Elena Rostov — The Field Surgeon
```json
{
  "id": "elena_rostov",
  "displayName": "Dr. Elena Rostov",
  "profession": "Trauma Surgeon",
  "bio": "Treated shrapnel wounds in frontline triage tents for years before the Exchange. Speaks in flat, clinical terms — patients are complex biological machines. It's the only way she knows to keep working.",
  "baseHealth": 90,
  "phantom_background_id": "nurse",
  "pre_war_profession_id": "nurse",
  "belief_profile_id": "atheist_rationalist",
  "personal_keepsake_item_id": "surgical_mask",
  "latentExpertTrait": "trait_healers_soul",
  "activeQuestlineId": "quest_the_ars_crisis",
  "baseTraits": ["trait_clinical_detachment", "trait_surgical_precision", "trait_chronic_cough"]
}
```

---

### Phase L3: SurvivorNarrativeArcSystem (1 New C# File)

**File**: `Assets/_Game/Narrative/SurvivorNarrativeArcSystem.cs`

A lightweight system that tracks each named survivor's progression through their narrative arc milestones. Extends the existing `PersonalQuestSystem` pattern.

```csharp
// Key additions:
// - Tracks which named survivors are active in the bunker
// - Monitors milestone triggers (crafting events, morale thresholds, deaths witnessed)
// - Advances arc stage when conditions met
// - Raises OnArcMilestoneReached(survivor, milestoneIndex, branchDirection)
// - Integrates with MoralBranchingSystem for empathy/numbness axis
```

**Survivor fields needed** (add to Survivor.cs):
```csharp
public int NarrativeArcMilestone;       // 0-3 for the 4-stage arc
public string NarrativeArcBranchId;     // which branch was chosen at milestone 2
public bool IsNarrativeArcComplete;
public float NarrativeStressAccumulation; // separate from leader stress
```

**Constants**:
```csharp
public const int ArcMilestoneDiscovery = 0;    // Starting state
public const int ArcMilestoneTrigger = 1;      // Mid-game crucible event fires
public const int ArcMilestoneCrisis = 2;       // Branching choice point
public const int ArcMilestoneResolution = 3;   // Final outcome
```

---

### Phase L4: Four Character Questlines (JSON Data)

**File**: `Assets/StreamingAssets/Data/narrative_questlines.json`

Each questline follows the 4-stage pattern: Discovery → Trigger → Crisis → Resolution.

#### Questline 1: The Cracked Floor (Aris Thorne)
```
Stage 1: Discovery — Shelter suffers structural crack event. Aris identifies the damage.
Stage 2: Investigation — Aris insists on working 24h shifts to repair it.
Stage 3: Crisis — Player must choose:
  Branch A (Pragmatic Acceptance): Force Aris to rest → gains Resilient Builder trait
  Branch B (Obsessive Strain): Let Aris overwork → gains Severe Tremors, +work speed, -health
Stage 4: Resolution — Aris builds Deep Aquifer OR dies during siege repair leaving blueprints
```

#### Questline 2: The Dying Signal (Maya Lin)
```
Stage 1: Discovery — Maya intercepts repeating emergency broadcast on 142.5 MHz
Stage 2: Investigation — Send expedition to Communications Array to trace the signal
Stage 3: Crisis — Signal is her brother's unit's automated death loop. Player chooses:
  Branch A (The Beacon): Channel grief into saving others → Voice of Hope trait
  Branch B (Static Collapse): Withdraw into silence → Catatonic Depression, hyper-focused perception
Stage 4: Resolution — Maya leads Unification Protocol OR broadcasts final rescue beacon
```

#### Questline 3: The Refugee Mass Influx (Victor Vance)
```
Stage 1: Discovery — Refugee family arrives at hatch during fallout storm
Stage 2: Tension — Vance recommends turning them away to preserve rations
Stage 3: Crisis — Player overrides or follows Vance's advice:
  Branch A (Restored Humanity): Admit refugees, Vance softens → Guardian Captain trait
  Branch B (Cold Efficiency): Turn away, Vance becomes Iron Sentinel → +20% defense, -shared meal morale
Stage 4: Resolution — Vance negotiates Peace Treaty OR leads preemptive strike
```

#### Questline 4: The ARS Crisis (Elena Rostov)
```
Stage 1: Discovery — Survivor contracts terminal Acute Radiation Syndrome
Stage 2: Triage — Elena calculates treatment will consume 80% of medical supplies
Stage 3: Crisis — Player decides:
  Branch A (Oath Restored): Save patient regardless → Healer's Soul trait, +50% bed rest recovery
  Branch B (Triage Logic): Let patient pass peacefully → Cold Triage trait, -30% medical costs, +death penalty
Stage 4: Resolution — Elena synthesizes Ash Rot Remedy OR sacrifices self in containment breach
```

---

### Phase L5: Faction Lore Encyclopedia (JSON Data)

**File**: `Assets/StreamingAssets/Data/faction_lore.json`

Expand the existing `FactionLoreVoiceLines.cs` with full ideological entries:

```json
[
  {
    "faction_id": "iron_garrison",
    "display_name": "The Iron Garrison",
    "ideology": "Military continuity, strict resource rationing, absolute martial law",
    "origin_story": "Formed from surviving remnants of the regional armed forces command structure. Colonel Voss assumed command at Bunker Sigma-7 within 72 hours of the Exchange. They view all non-military survivors as civilian dependents who must contribute labor or food to the defense effort.",
    "key_beliefs": [
      "Civilization is maintained by supply chains and ammunition counts",
      "Democracy is a luxury of peacetime",
      "Every mouth must earn its rations through labor or combat service"
    ],
    "dialogue_style": "Formal, terse, military acronyms, cold operational efficiency",
    "signature_quote": "Civilization isn't built on sympathy; it's maintained by supply chains and ammunition counts.",
    "relationship_matrix": {
      "ash_militia": "hostile",
      "cult_of_ash_sign": "suspicious",
      "warlords_sector_4": "hostile"
    },
    "tribute_demands": ["food_rations", "young_recruits"],
    "tech_offerings": ["artillery_support", "military_grade_filtration", "armored_patrol_escort"]
  },
  {
    "faction_id": "ash_militia",
    "display_name": "The Ash Militia",
    "ideology": "Local democracy, resource sharing, mutual defense, civilian autonomy",
    "origin_story": "A coalition of pre-war farmers, miners, teachers, and tradespeople who banded together when both raider gangs and Garrison conscription units threatened their communities. They hold weekly council meetings in the old Grange Hall, lit by salvaged oil lamps.",
    "key_beliefs": [
      "If we turn into monsters just to survive, the war already won",
      "Everyone gets a vote and everyone gets a share",
      "The old world ended — we decide what replaces it"
    ],
    "dialogue_style": "Informal, warm, pragmatic, weary but determined",
    "signature_quote": "If we turn into monsters just to survive the fallout, then the war already won.",
    "relationship_matrix": {
      "iron_garrison": "hostile",
      "cult_of_ash_sign": "neutral",
      "warlords_sector_4": "hostile"
    },
    "tribute_demands": [],
    "tech_offerings": ["civilian_medical_supplies", "seed_exchange", "trade_network_access"]
  },
  {
    "faction_id": "cult_of_ash_sign",
    "display_name": "The Cult of the Ash Sign",
    "ideology": "Apocalyptic purification, radiation worship, ascetic martyrdom",
    "origin_story": "A fanatical religious movement that arose in the high-fallout zones where radiation levels were too lethal for military patrols. Their prophet, known only as The Vessel, emerged from a destroyed reactor site unscathed — or so they claim. They view radiation sickness as a spiritual ordeal that purifies the soul.",
    "key_beliefs": [
      "The nuclear fire was divine judgment on a corrupt world",
      "Radiation is the breath of the new god — breathe it and be cleansed",
      "Death by fallout is not death; it is ascension"
    ],
    "dialogue_style": "Arcane, rhythmic, serene, unsettlingly peaceful amidst horror",
    "signature_quote": "Do not fear the glow, child. The fire burned away the old world's lies. Drink the ash and be renewed.",
    "relationship_matrix": {
      "iron_garrison": "suspicious",
      "ash_militia": "neutral",
      "warlords_sector_4": "neutral"
    },
    "tribute_demands": ["ritual_participation"],
    "tech_offerings": ["rad_resistant_herbal_remedy", "fallout_zone_navigation", "ghoul_deterrent_herbs"]
  },
  {
    "faction_id": "warlords_sector_4",
    "display_name": "The Warlords of Sector 4",
    "ideology": "Mercenary opportunism, trade control, survival of the fittest",
    "origin_story": "A loose syndicate of armed scavengers, former convicts, and ex-mercenaries who control key highway bottlenecks, fuel depots, and river crossings. They have no loyalty beyond the contract. Their leader, The Tollman, rose to power by being the only person who knew the bridge demolition codes.",
    "key_beliefs": [
      "Clean water costs blood or bullets — pick which you're paying with",
      "Loyalty is a tradable commodity, like diesel or antibiotics",
      "The strong don't survive because they're strong — they survive because they're useful"
    ],
    "dialogue_style": "Rough, cynical, transactional, threatening",
    "signature_quote": "Clean water costs blood or bullets. Pick which one you're paying with today.",
    "relationship_matrix": {
      "iron_garrison": "hostile",
      "ash_militia": "hostile",
      "cult_of_ash_sign": "neutral"
    },
    "tribute_demands": ["fuel", "ammunition", "medical_kits"],
    "tech_offerings": ["bridge_passage", "smuggled_goods", "mercenary_contracts"]
  }
]
```

---

### Phase L6: Narrative Arc Events (15 Events in events.json)

Add 15 new narrative events to `events.json`. These are the branch-point moments for each character arc. Examples:

| Event ID | Character | Stage | Description |
|----------|-----------|-------|-------------|
| `narrative_aris_structural_crack` | Aris | Trigger | Shelter develops a structural crack. Aris identifies the damage. |
| `narrative_aris_overwork_crisis` | Aris | Crisis | Aris insists on 24h shifts. Force rest or let him push? |
| `narrative_aris_resolution_aquifer` | Aris | Resolution | Aris completes the Deep Aquifer — or collapses trying. |
| `narrative_maya_signal_discovery` | Maya | Discovery | Faint 142.5 MHz signal detected with familiar pattern. |
| `narrative_maya_brothers_loop` | Maya | Crisis | Signal is her brother's death loop. Grief or resolve? |
| `narrative_maya_unification` | Maya | Resolution | Maya leads the broadcast that unites the enclaves. |
| `narrative_vance_refugee_arrival` | Vance | Trigger | Refugee family at hatch during fallout storm. |
| `narrative_vance_command_override` | Vance | Crisis | Override Vance's recommendation or follow it? |
| `narrative_vance_peace_treaty` | Vance | Resolution | Vance brokers treaty or leads preemptive strike. |
| `narrative_elena_ars_diagnosis` | Elena | Trigger | Survivor diagnosed with terminal ARS. |
| `narrative_elena_supply_choice` | Elena | Crisis | Treat at 80% supply cost or let pass peacefully? |
| `narrative_elena_ash_rot_cure` | Elena | Resolution | Synthesize cure or sacrifice self in containment breach. |
| `narrative_garrison_defector_arrival` | Any | Lore | A Garrison defector arrives with intel and a story. |
| `narrative_cult_prophet_sighting` | Any | Lore | Cult members report their prophet walking through a reactor site. |
| `narrative_militia_council_invitation` | Any | Lore | Militia invites the player to attend a council meeting. |

---

### Phase L7: GameBootstrap Wiring (C#)

**New file**: `Assets/_Game/Core/GameBootstrap.DeepLoreWiring.cs`

Wires:
- `SurvivorNarrativeArcSystem` construction + tick registration
- Character arc milestone listeners on existing events:
  - Crafting completions → check for Aris milestone triggers
  - Radio frequency decodes → check for Maya milestone triggers
  - Hatch defense outcomes → check for Vance milestone triggers
  - Medical treatments → check for Elena milestone triggers
- Narrative event queue triggers at milestone boundaries
- Integration with `MoralChronicleBridge` for endgame narrative generation

---

### Phase L8: Expansion Survivor Fields JSON

**File**: `Assets/StreamingAssets/Data/deep_lore_survivor_fields.json`

Map the 4 new survivors with their expansion fields (same format as `expansion_survivor_fields.json`).

---

## III. FILE MANIFEST

### New C# Files (2)
```
Assets/_Game/Narrative/SurvivorNarrativeArcSystem.cs
Assets/_Game/Core/GameBootstrap.DeepLoreWiring.cs
```

### New JSON Files (5)
```
Assets/StreamingAssets/Data/world_history.json              (~25 entries across 4 eras)
Assets/StreamingAssets/Data/narrative_questlines.json       (4 questlines, 4 stages each)
Assets/StreamingAssets/Data/faction_lore.json               (4 factions, full ideology + lore)
Assets/StreamingAssets/Data/deep_lore_survivor_fields.json  (4 survivors expansion mapping)
Assets/StreamingAssets/Data/narrative_arc_events.json       (15 events for branch points)
```

### Modified Files (4)
```
Assets/_Game/Survivors/Survivor.cs             — add NarrativeArcMilestone, NarrativeArcBranchId, etc.
Assets/StreamingAssets/Data/survivors.json     — add 4 named survivor entries
Assets/StreamingAssets/Data/events.json        — add 15 narrative arc events
Assets/_Game/Core/GameBootstrap.InitializeSystems.cs — add InitDeepLore() call
```

### New Survivor Fields (4)
```csharp
public int NarrativeArcMilestone;
public string NarrativeArcBranchId;
public bool IsNarrativeArcComplete;
public float NarrativeStressAccumulation;
```

---

## IV. UI WIDGETS FOR CURSOR (3 Widgets)

### Widget 1: LoreCodexPanel
- **Purpose**: Encyclopedia of discovered world history, faction lore, and character backgrounds
- **UXML**: Tabbed panel with "History" / "Factions" / "Characters" tabs
- **Data**: `world_history.json`, `faction_lore.json`, survivor bio fields
- **Canva assets**: Parchment/terminal texture, faction emblem icons (reuse from Exp 3), era timeline icons

### Widget 2: FactionRelationshipMap
- **Purpose**: Visual diagram of faction relationships (the diagram from the spec — Iron Garrison ↔ Ash Militia ↔ Cult ↔ Warlords)
- **UXML**: Node graph with faction circles connected by colored relationship lines
- **Data**: `faction_lore.json` relationship_matrix
- **Canva assets**: Larger faction emblem icons (64×64), relationship line textures (hostile=red, neutral=grey, allied=blue)

### Widget 3: CharacterArcProgressPanel
- **Purpose**: Shows each named survivor's narrative arc stage, milestone history, and branch taken
- **UXML**: Vertical timeline with 4 stage circles, branch labels at crisis point
- **Data**: `SurvivorNarrativeArcSystem` state, `PersonalQuestSystem` quest progress
- **Canva assets**: Stage circle icons (reuse from Exp 3 quest tracker), branch arrow icons

---

## V. CANVA ASSET REQUIREMENTS

| # | Asset Name | Size | Type | Used By |
|---|-----------|------|------|---------|
| 1 | `texture_parchment_bg` | 512×512 | PNG | LoreCodexPanel background |
| 2 | `texture_terminal_screen` | 512×512 | PNG | LoreCodexPanel alt background |
| 3 | `icon_era_pre_exchange` | 32×32 | SVG | LoreCodexPanel timeline |
| 4 | `icon_era_hour_zero` | 32×32 | SVG | ^ |
| 5 | `icon_era_black_sky` | 32×32 | SVG | ^ |
| 6 | `icon_era_ashfall` | 32×32 | SVG | ^ |
| 7 | `icon_hostile_relation` | 16×16 | SVG | FactionRelationshipMap connector |
| 8 | `icon_neutral_relation` | 16×16 | SVG | ^ |
| 9 | `icon_allied_relation` | 16×16 | SVG | ^ |
| 10 | `icon_arc_branch_a` | 24×24 | SVG | CharacterArcProgressPanel branch marker |
| 11 | `icon_arc_branch_b` | 24×24 | SVG | ^ |
| 12 | `icon_arc_complete` | 32×32 | SVG | CharacterArcProgressPanel final stage |

---

## VI. IMPLEMENTATION ORDER (Pi — 3 Days)

### Day 1: Data Foundation
1. Add 4 `NarrativeArc*` fields to Survivor.cs
2. Create `world_history.json` — 25 entries across 4 eras
3. Create `faction_lore.json` — 4 full faction entries
4. Add 4 survivor entries to survivors.json
5. Create `deep_lore_survivor_fields.json`

### Day 2: Systems + Questlines
6. Create `SurvivorNarrativeArcSystem.cs`
7. Create `narrative_questlines.json` — 4 questlines
8. Create `narrative_arc_events.json` — 15 events
9. Add events to events.json (or reference from the separate file)

### Day 3: Wiring + Tests
10. Create `GameBootstrap.DeepLoreWiring.cs`
11. Add `InitDeepLore()` call to InitializeSystems
12. Write 15+ EditMode tests for arc progression, milestone triggers, branch selection
13. Update `INTEGRATION_MASTER_PLAN.md`

---

## VII. EDITMODE TESTS

| Test | What It Verifies |
|------|-----------------|
| `NarrativeArc_ArisMilestone1_TriggersOnCraftCompletion` | Aris at milestone 0 → craft valve → advances to milestone 1 |
| `NarrativeArc_MayaMilestone1_TriggersOnRadioDecode` | Maya decodes frequency → advances to milestone 1 |
| `NarrativeArc_VanceBranchA_GrantsGuardianCaptain` | Choose Branch A for Vance → trait_guardian_captain granted |
| `NarrativeArc_ElenaBranchB_GrantsColdTriage` | Choose Branch B for Elena → trait_cold_triage granted |
| `NarrativeArc_Complete_SetsFlag` | Milestone 3 reached → IsNarrativeArcComplete = true |
| `WorldHistory_AllEntries_ParseCorrectly` | world_history.json → all 25 entries have required fields |
| `FactionLore_AllFactions_HaveRequiredFields` | faction_lore.json → 4 factions with ideology, beliefs, quotes |
| `SurvivorEntries_CorrectBaseTraits` | Aris has [methodical, insomniac, guilt_ridden] |
| `Questlines_AllFour_HaveFourStages` | Each questline has exactly 4 stages with 2 branches at stage 2 |
| `ArcStressAccumulation_IncreasesOnDeathWitnessed` | Vance witnesses death → NarrativeStressAccumulation increases |

---

## VIII. FOLLOW-UP PROMPT

> *"Proceed with Deep Lore Phase L1 — add NarrativeArcMilestone, NarrativeArcBranchId, IsNarrativeArcComplete, NarrativeStressAccumulation to Survivor.cs, then create world_history.json with 25 entries across all 4 eras."*
