# C1 — Flagship Integration Plan [45]: Master Blueprints, Captive Rehabilitation, Seasonal Preservation & Dynamic Legacy Epilogue

> **Output:** `C1_planintegration[45].md`
>
> **Scope:** Task 5 Pre-War Codex Reconstruction & Technological Blueprints; Task 6 Captive Rehabilitation, Labor Unions & Escape Conspiracies; Task 7 Seasonal Food Preservation, Smoking Chambers & Winter Starvation Buffer; Task 8 Dynamic Campaign Epilogue Branching & Legacy Timeline Chronicle.
>
> **Primary mission:** deepen late-game progression and campaign consequence while preserving single authority across archives, research, recipes, construction, factions, prisoners, citizenship, schedules, food, nutrition, illness, sanitation, audio, vehicles, moral choices, and epilogue scoring.
>
> **Core doctrine:** every requested integration must compose existing systems rather than create parallel truth. Archive decryption may reveal blueprint IDs, but Foundry/Workshop/Construction authorities own recipe and project availability. Captive systems may track custody, conspiracy, rehabilitation, and oath eligibility, but citizenship, survivor identity, schedule, relations, trauma, and faction exchange remain canonical elsewhere. Food preservation owns preservation state and shelf life, not winter temperature, nutrition, disease, or hygiene. `CampaignEpilogueEngine` derives legacy from canonical campaign facts; it does not become a second moral, government, survivor, vehicle, knowledge, or trauma database.
>
> **Mandatory execution order:** P0 authority audit → Task 5 archive blueprint/codex architecture → Task 6 captive conspiracy/rehabilitation/citizenship → Task 7 seasonal preservation/storage/nutrition → Task 8 epilogue composite legacy/timeline/NG+ → cross-system persistence → determinism → long-horizon balance → accessibility/localization → CI/SHIP gate.
>
> **High-risk corrections to the raw tasks:**
> 1. A decrypted master blueprint should **unlock eligibility** for recipes/projects through `ResearchSystem`/TechTree/content gates; `PrewarArchiveDecryptionSystem` must not directly toggle recipe visibility fields in `foundry_production.json` or `workshop_recipes.json` at runtime.
> 2. Multiple specialists should contribute through a canonical assignment/work-effort contract; archive logic must not duplicate skills, scheduler state, or survivor availability.
> 3. Factions cannot magically know a decrypted archive exists. Envoys, theft attempts, or trade demand require an information-propagation path such as Plan 131 rumor/intelligence, witnessed events, spies, or direct disclosure.
> 4. Archive trading should use canonical trade/reputation/treaty authorities and copy/provenance rules. Never sell the same unique physical archive repeatedly without a real duplication/copy mechanism.
> 5. Captive harshness and escape-cabal formation must use real detention conditions, security, guard duty, relations, and autonomy. “Held >30 days” is a trigger window, not sufficient proof of conspiracy.
> 6. The proposed `<150 security` threshold and `800 loyalty` oath threshold are tuning candidates until verified against actual scales.
> 7. Vocational rehabilitation does not convert prisoners into free labor inventory. Training consumes real time, uses skill progression, and must respect custody/ethics/governance.
> 8. Citizen integration must pass through the canonical survivor/citizenship identity transition. `ShelterPrisonerSystem` cannot retain a person as both captive and citizen.
> 9. Winter freezing must be resolved by `YearOfAshSystem` + canonical storage/thermal exposure, not by `FoodPreservationSystem` inventing a second temperature model.
> 10. “Varied diet bonus for immunity” and scurvy/malnutrition need medical/nutrition authority. Food preservation may expose dietary composition and reserve state but must not directly mutate disease resistance.
> 11. Spoilage odor can feed sanitation/air/hygiene systems, but `FoodPreservationSystem` must not independently apply disease/hygiene penalties already owned there.
> 12. `Shelter Legacy Rating` is a derived epilogue metric, not a new campaign authority that other systems mutate during play.
> 13. “Low average trauma” should not be computed by averaging opaque trauma token counts if the mental-health system exposes a better normalized post-campaign health/readiness metric.
> 14. New Game Plus inheritance must not serialize a whole prior campaign into the next run. It should generate a compact versioned legacy seed/profile with bounded starting modifiers.
> 15. Procedural epitaphs must be deterministic from canonical survivor outcomes and localization templates, not uncontrolled free-form generation.

---

# 0. Non-Negotiable Authorities

## Archive / Research
- `PrewarArchiveDecryptionSystem` owns archive condition, cleaning stage, decoding progress, specialist effort input, reconstructed fragment state, and blueprint discovery state.
- `PrewarArchiveCatalog` / archive data owns authored archive definitions.
- `ResearchSystem` owns research knowledge eligibility.
- `TechTree` owns technology unlock state.
- Foundry production authority owns foundry recipe availability.
- Workshop recipe authority owns workshop recipe availability.
- construction/project authority owns construction project availability.
- item/inventory authority owns chemicals and physical archive copies.
- faction/rumor/intelligence authorities own who knows about the archive.
- trade/reputation/treaty authorities own sale effects.
- Codex/Journal/content reader owns player-facing lore presentation/storage.
- `CampaignEpilogueEngine` may read unlocked technologies but cannot unlock them.

## Captives
- `ShelterPrisonerSystem` owns captive status, detention state, conspiracy state, rehabilitation linkage, parole state, and exchange eligibility while the person is captive.
- survivor identity authority owns canonical person/survivor entity.
- schedule/duty authority owns guard and vocational assignments.
- SkillProgression owns apprentice skill growth.
- security/room authority owns detention security.
- relations/autonomy owns social behavior and interpersonal state.
- faction/diplomacy authority owns prisoner exchange consequences.
- mental-health authority owns trauma/stress/recovery.
- citizenship/survivor roster authority owns citizen conversion.
- audio director owns unrest acoustic intent.

## Food
- `FoodPreservationSystem` owns preservation method, cohort shelf life, degradation/spoilage stage, and preservation reserves.
- `YearOfAshSystem` owns season/winter state.
- shelter thermal/storage authority owns room/container temperature and insulation.
- crafting/build authority owns smokehouse/canner construction.
- recipe authority owns recipes.
- inventory owns food item quantities.
- nutrition authority owns dietary composition/deficiency.
- medical authority owns scurvy/malnutrition disease/affliction.
- sanitation/air authority owns hygiene/odor/disease-vector consequences.
- audio director owns smokehouse/canning acoustic presentation.
- epilogue may read reserves but never mutate them.

## Epilogue
- `CampaignEpilogueEngine` owns vignette selection, legacy-score derivation, timeline projection, epitaph projection, and NG+ legacy package generation.
- it does not own campaign facts.
- all categories are computed from canonical data.
- engine references in Core remain zero.
- presentation/audio lives in Godot host.
- NG+ bonus consumption occurs at new-campaign initialization, not inside old campaign state.

---

# 1. Definition of Done

The bundle closes only when:

- 8 master blueprint archives are data-driven, reachable, localized, and integrity-valid;
- chemical cleaning prerequisites use canonical item IDs and transactional consumption;
- multi-specialist decoding uses canonical skills + schedule + work-effort;
- blueprint discovery flows into Research/TechTree and then into foundry/workshop/project eligibility exactly once;
- Codex lore fragments unlock through stable content IDs;
- faction reaction requires actual information propagation;
- archive copies/trading use canonical item/provenance/economy rules;
- blueprint unlock fanfare is optional presentation and headless-safe;
- epilogue can detect mastered technologies without owning them;

- captive psychological/conspiracy traits are valid content;
- escape cabals form from deterministic risk accumulation tied to real custody conditions;
- low security/negligent guards influence risk through one security projection;
- escape attempts use canonical room/security/inventory/schedule/world transition logic;
- vocational rehabilitation uses SkillProgression and schedule;
- oath/citizenship transition is atomic and mutually exclusive with captive state;
- prisoner exchange uses faction/diplomacy authority;
- unrest audio is presentation-only;
- captive trauma recovery uses mental-health authority;
- 60-day custody/citizenship lifecycle test passes;

- smoking/canning tiers are data-driven;
- smokehouse/canner room/build prerequisites resolve;
- 90-day shelf-life recipes are real recipe/item entries;
- winter exposure derives from YearOfAsh + storage thermal state;
- insulation modifies canonical storage thermal protection;
- nutrition panel shows composition/reserve information without owning deficiency;
- scurvy/malnutrition are medical/nutrition outcomes;
- spoilage odor feeds sanitation/air exactly once;
- 50+ cohort save roundtrip is deterministic;

- 12 new epilogue vignettes are added and all text keys resolve;
- Environmental Recovery, Faction Sovereignty, Cultural Memory, Ethical Legacy categories are supported;
- moral/governance facts are read from canonical history;
- Shelter Legacy Rating is fully documented and reproducible;
- timeline milestones derive from semantic campaign events;
- survivor epitaphs are deterministic/localized;
- NG+ legacy seed/profile is compact and versioned;
- vehicle/mental-health/technology/food achievements derive from real end-state;
- all vignette selections are deterministic and priority-stable;
- `CampaignEpilogueEngine` contains zero engine references.

---

# 2. P0 — Forensic Authority Audit

Before changing any implementation, inspect:

## 2.1 Archive
- `PrewarArchiveDecryptionSystem`
- `PrewarArchiveSaveStore`
- `PrewarArchiveCatalog`
- `Assets/StreamingAssets/Data/prewar_archives.json`
- `foundry_production.json`
- `workshop_recipes.json`
- ResearchSystem
- TechTree
- construction/project unlock authority
- inventory transaction APIs
- chemicals/purified alcohol/acid neutralizer IDs
- assignment/schedule APIs
- Engineer/Scholar skill sources
- Codex reader/catalog
- faction information propagation
- trade/reputation/treaty systems
- `ShelterAcousticDirector`
- epilogue technology inputs

## 2.2 Captives
- `ShelterPrisonerSystem`
- `ShelterPrisonerSaveStore`
- `captive_interrogations.json`
- security score source/scale
- guard duty/negligence model
- cell/room containment
- inventory/tool theft semantics
- surface transition/escape handling
- apprentice/SkillProgression system
- labor/work assignment
- loyalty/trust scale
- parole/citizenship transition
- faction officer/prisoner exchange contracts
- hostage return system if present
- mental-health integration
- acoustic director
- prisoner dialogue catalog

## 2.3 Food
- `FoodPreservationSystem`
- `FoodPreservationSaveStore`
- `food_preservation.json`
- recipe system
- `KitchenNutritionPanel`
- `YearOfAshSystem`
- shelter thermal/storage model
- room upgrades/build system
- insulation materials/items
- nutrition/micronutrient model
- medical affliction catalog
- sanitation/hygiene system
- spoilage/odor/air quality model
- acoustic director
- epilogue food metrics

## 2.4 Epilogue
- `CampaignEpilogueEngine`
- `campaign_epilogues.json`
- current 4 + proposed 12 vignette model
- moral-choice history
- governance/autocracy/council truth
- survivor roster/history
- knowledge/codex/research metrics
- faction sovereignty state
- vehicle fleet status
- mental-health aggregate API
- food reserve API
- timeline/history event layer
- new campaign initialization
- save seed/profile support
- Godot `CampaignEpilogueModal`
- epilogue audio bridge.

Create:
`docs/architecture/C1_45_AUTHORITY_MATRIX.md`

Columns:
```text
fact | canonical owner | read API | command/write API | persisted? | integration role | status
```

Create ADRs:
- `ADR_MASTER_BLUEPRINT_UNLOCK_PIPELINE.md`
- `ADR_ARCHIVE_INFORMATION_AND_FACTION_REACTION.md`
- `ADR_CAPTIVE_TO_CITIZEN_ATOMIC_TRANSITION.md`
- `ADR_CAPTIVE_CONSPIRACY_SECURITY_MODEL.md`
- `ADR_SEASONAL_FOOD_STORAGE_THERMAL_BOUNDARY.md`
- `ADR_NUTRITION_DEFICIENCY_AUTHORITY.md`
- `ADR_EPILOGUE_LEGACY_RATING_AS_DERIVED_METRIC.md`
- `ADR_NEW_GAME_PLUS_LEGACY_PACKAGE.md`

---

# 3. Task 5 — Pre-War Codex Reconstruction & Technological Blueprints

## 3.1 Eight master blueprint archives

Extend:
`Assets/StreamingAssets/Data/prewar_archives.json`

Required named concepts:
1. Geothermal Heat Exchangers
2. Hydroponic Nutrient Synthesis
3. Magnetic Rail Propulsion
4. Pulse Radars

Add four additional master-tier archive concepts that fit existing tech tree and actual consumers, for example:
5. High-Efficiency Induction Foundry Control
6. Advanced Radiation Shielding Laminates
7. Closed-Loop Water Reclamation
8. Precision Munitions/Metallurgy Control

Do not add a blueprint if no canonical recipe/project/tech consumer exists or can be legitimately wired.

Suggested schema:

```text
archive_id
display_key
archive_tier
physical_media_type
condition_profile
cleaning_stages[]
cipher_stages[]
specialist_requirements[]
effort_model
fragment_ids[]
blueprint_unlock_ids[]
research_unlock_ids[]
tech_unlock_ids[]
construction_project_ids[]
faction_interest_tags[]
trade_copy_policy
audio_cue_ids[]
epilogue_tags[]
localization_keys[]
```

## 3.2 Chemical cleaning

Required candidates:
- `chemicals`
- `purified_alcohol`
- `acid_neutralizer`

Verify exact IDs.

Each cleaning stage declares:
```text
required_item_id
quantity
condition_range
consumption_timing
failure_policy
```

Transaction:
1. archive action validated;
2. chemical reserved;
3. item consumed;
4. cleaning state committed;
5. semantic event emitted.

No UI/direct catalog mutation.

If chemical is missing:
- stage blocked;
- no progress;
- no partial consumption.

## 3.3 Multi-specialist decoding

Raw target: Engineer + Scholar.

Use canonical role/skill predicates:
```text
specialist_slots[]
  role_or_skill_tag
  minimum_level
  effort_weight
```

Assignment must:
- reserve survivor schedule;
- respect health/fatigue;
- prevent simultaneous conflicting work;
- permit replacement;
- restore after save.

Do not persist duplicate skill values.

## 3.4 Effort-point progression

Suggested deterministic formula:

```text
daily_effort =
 Σ specialist_contribution
 × archive_condition_multiplier
 × tool/facility_multiplier
 × bounded_rng_factor_if_uncertainty_is_designed
```

If RNG is used:
- seeded by campaign/archive/stage/day;
- same seed + same assignments = same progress.

Prefer integer effort points/permille.

Do not use wall clock.

## 3.5 Blueprint unlock pipeline

Correct:

```text
archive stage completed
→ blueprint_discovered event
→ ResearchSystem/TechTree grants canonical knowledge/tech state
→ recipe/project authorities evaluate unlock predicates
→ foundry/workshop/construction content becomes available
→ UI notification
```

Do not edit JSON at runtime.

The JSON files define authored content with unlock predicates such as:
```text
requires_tech: tech_geothermal_exchange
```

## 3.6 Foundry/workshop recipes

For every blueprint:
- map to at least one real recipe/project;
- validate ingredient IDs;
- validate facility requirements;
- validate skill requirements;
- validate unlock tech ID;
- prove content reachability.

Create:
`docs/PREWAR_BLUEPRINT_RECIPE_MATRIX.md`.

## 3.7 Codex lore

Fragments should use:
- stable fragment IDs;
- translation keys;
- unlock stage;
- archive provenance.

Flow:
```text
decoded fragment
→ Codex authority marks entry known
→ reader displays
```

No long lore strings inside save state.

## 3.8 Faction reactions

Critical information rule:
Faction reaction requires one of:
- player discloses archive;
- spy/intelligence leak;
- envoy witnessed demonstration;
- rumor/intelligence propagation;
- trade offer;
- allied research exchange.

No omniscient faction reaction on decryption completion.

Reaction flow:
```text
knowledge propagation
→ faction learns blueprint/archive exists
→ diplomacy/faction AI considers envoy/purchase/theft
→ mission/event system instantiates response
```

## 3.9 Archive trading

Define whether trade unit is:
- original archive;
- physical duplicate/copy;
- transcript/blueprint copy;
- licensed knowledge exchange.

Never repeatedly sell one unique physical spool.

Copy creation, if supported:
- consumes medium/resources/time;
- creates canonical item/info artifact;
- provenance records source archive;
- no duplicate-tech exploit.

Reputation/treaty effects:
- faction/diplomacy owns.
- “massive” boost must be tuning data.
- treaty perks require treaty authority, not archive system.

## 3.10 Theft attempts

If factions know archive exists:
- event/quest/espionage system may generate theft attempt.
- archive system only exposes target/provenance.
- security/stealth/combat systems resolve theft.

Do not implement theft entirely inside archive decryption.

## 3.11 Audio fanfare

On canonical blueprint unlock:
- emit synthesized chime/reel cue request.
- `ShelterAcousticDirector` resolves audio.
- optional audio failure non-fatal.
- headless no-op.

## 3.12 Epilogue hook

Expose canonical epilogue facts:
```text
master_blueprints_decrypted_count
specific_master_tech_ids[]
research_projects_completed[]
advanced_projects_constructed[]
```

“Technological Golden Age” should require actual technology adoption, not merely owning an archive if design intends material transformation.

Recommended criteria:
- minimum master-tech count;
- at least one advanced project built;
- knowledge preservation threshold;
- shelter survival threshold.

## 3.13 Tests

Create:
`PrewarArchiveTechUnlockTests.cs`.

Test:
- 8 archive definitions load;
- chemical prerequisites;
- multi-specialist assignment;
- deterministic effort;
- one specialist missing;
- archive cleaning transaction;
- blueprint event exactly once;
- ResearchSystem consumes;
- TechTree consumes;
- foundry recipe unlock;
- workshop recipe unlock;
- construction project unlock where applicable;
- Codex fragment unlock;
- save/load;
- no duplicate unlock on restore;
- faction reaction absent without knowledge propagation;
- faction reaction present after legitimate disclosure/leak;
- archive copy/trade cannot duplicate unique original.

Update:
`docs/PREWAR_ARCHIVE_AUTHORITY_MAP.md`.

---

# 4. Task 6 — Captive Rehabilitation, Labor Unions & Escape Conspiracies

## 4.1 Extend captive profile data

Extend:
`Assets/StreamingAssets/Data/captive_interrogations.json`

Add data fields only if they do not duplicate survivor traits:

```text
captive_profile_id
conspiracy_tags[]
custody_response_tags[]
rehabilitation_tags[]
vocational_affinities[]
exchange_value_tags[]
oath_eligibility_tags[]
dialogue_keys[]
```

If “psychological profiles” correspond to actual survivor traits/mental-health state:
- reference canonical trait/mental-health IDs;
- do not copy values.

## 4.2 Conspiracy state

`ShelterPrisonerSystem` may own:
```text
cabal_id
member_ids[]
formation_day
risk_accumulator
known_to_player
detected
active_plan_stage
escape_window
processed_events[]
```

Do not store duplicate guard/security state.

## 4.3 Formation model

Raw:
- harsh conditions >30 days;
- security <150;
- negligent guards.

Flagship:
`30 days`, `<150`, and negligence multipliers are data/tuning.

Risk inputs:
- duration in custody;
- food/comfort treatment;
- overcrowding;
- abuse/harshness if modeled;
- parole prospects;
- relations among captives;
- guard coverage;
- cell security;
- tool access;
- faction loyalty;
- recent failed escape;
- rehabilitation progress.

Use deterministic daily/event boundary, not per-frame RNG.

## 4.4 Cabal formation

Require:
- at least 2 compatible captives unless lone escape conspiracies supported separately;
- social/contact opportunity;
- bounded risk threshold;
- deterministic seeded resolution.

Do not automatically create cabal on Day 31.

## 4.5 Escape attempt state machine

Suggested:

```text
DORMANT
→ ORGANIZING
→ ACQUIRING_TOOLS
→ TIMING_ESCAPE
→ BREACH
→ INTERNAL_MOVEMENT
→ SURFACE_RUSH
→ ESCAPED / RECAPTURED / SURRENDERED / FAILED
```

Each phase uses canonical systems.

## 4.6 Tool theft

Escape attempt may request:
- workshop tool theft.

Inventory authority owns item transfer.

Flow:
```text
cabal action
→ theft intent
→ access/security check
→ item transfer if successful
→ provenance event
```

No “virtual prybar” stored only in cabal state.

## 4.7 Cell breach

Security/room authority resolves:
- locks;
- barriers;
- guard coverage.

Prisoner system orchestrates but does not duplicate security score.

## 4.8 Surface rush

World/shelter transition authority resolves:
- exits;
- guard interception;
- combat/nonviolent containment.

Avoid auto-kill resolution in prisoner system.

## 4.9 Vocational rehabilitation

Create a positive pathway.

Use:
- canonical schedule;
- apprentice/work assignment;
- SkillProgression;
- mentor/craftsman availability;
- custody status.

Potential tracks:
- workshop;
- maintenance;
- cooking/preservation;
- farming;
- logistics.

Training should:
- consume time;
- require mentor/workstation;
- increment canonical skill;
- affect rehabilitation/loyalty through bounded semantic events.

No free labor bonus.

## 4.10 “Labor unions”

Do not invent a separate union system unless Plan 63 already defines organized labor.

Interpret the user’s target as:
- captive work groups;
- apprentice cohorts;
- collective grievances;
- bargaining/conditions events.

If a real labor governance system exists:
- integrate.
Otherwise:
- defer formal union mechanics as follow-on.

## 4.11 Loyalty

Verify actual scale before using `800`.

If captive loyalty already exists:
- reuse.

If not:
- do not create a second generic loyalty meter if relations/trust/faction adherence already expresses it.
- write ADR.

Oath eligibility should derive from:
- rehabilitation milestones;
- trust/relations;
- time;
- conduct;
- faction risk;
- player governance;
- survivor consent.

## 4.12 Oath of the Holdfast

Atomic transition:

```text
validate captive
validate oath eligibility
validate survivor roster/citizenship capacity
release prisoner obligations
create/update canonical citizen status
remove from active captive collections
migrate schedule assignment refs
retain person identity/history
emit citizenship event
```

Postcondition:
- person is not simultaneously `captive` and `citizen`.

No duplicate survivor ID.

## 4.13 Mental-health integration

Paroled/rehabilitating captives may carry:
- hostile trauma;
- captivity trauma;
- guilt;
- fear.

Peer support:
- is a recovery action/social event;
- mental health owns recovery.

Prisoner system can emit:
```text
rehabilitation_social_support
```

Do not directly remove trauma.

## 4.14 Ethical prisoner exchange

Player decision:
- return captured enemy officer;
- refuse/retain;
- negotiate.

Diplomacy/faction authority owns:
- goodwill;
- treaty progress;
- hostage return;
- retaliation.

Prisoner system owns:
- officer custody and release execution.

If hostages are returned:
- use canonical survivor/faction transfer.

## 4.15 Labor vs exchange tradeoff

Do not hardcode:
“refusing = labor output bonus.”

Refusing simply retains the captive; any labor contribution depends on actual assignment and consent/governance model.

## 4.16 Unrest audio

Cabal/unrest semantic cues:
- metallic bunk banging;
- hushed whispering.

`ShelterAcousticDirector` handles presentation.

Audio:
- cannot increase cabal risk;
- cannot reveal secret cabal unless player has detection/ambient rationale;
- hidden whisper cue must obey information design.

## 4.17 Determinism

Seed:
```text
campaign_seed
+ captive_group/cabal_id
+ day
+ event_sequence
```

Same input:
- same formation;
- same escape decision;
- same oath conversion if stochastic.

Prefer deterministic eligibility + seeded uncertainty.

## 4.18 Save

`ShelterPrisonerSaveStore` persists:
- cabal state;
- rehabilitation/vocational linkage;
- processed escape events;
- oath eligibility milestone refs.

Do not duplicate:
- skills;
- inventory;
- room security;
- guard schedules;
- mental-health state;
- faction state;
- citizen survivor state.

## 4.19 60-day test

`CaptiveRehabilitationAndEscapeTests.cs`

Scenarios:
1. humane secure custody → low cabal risk;
2. harsh weak-security custody → cabal forms;
3. tool theft attempt;
4. failed breach;
5. successful escape path;
6. vocational rehabilitation;
7. mentor unavailable;
8. parole;
9. peer support;
10. oath transition;
11. officer exchange;
12. refusal path;
13. save/load mid-cabal;
14. save/load mid-vocational program.

Assertions:
- deterministic;
- no duplicate captive/citizen;
- no negative inventory;
- no duplicate skill award;
- no phantom guard state;
- no audio leak of hidden cabal;
- faction effects exactly once.

Update:
`docs/CAPTIVE_SYSTEM_AUTHORITY_MAP.md`.


---

# 5. Task 7 — Seasonal Food Preservation, Smoking Chambers & Winter Starvation Buffer

## 5.1 Mission

Deepen `FoodPreservationSystem` so the player can deliberately build winter resilience through smoking, canning, insulation, reserve planning, and diet diversity, while YearOfAsh, room temperature, inventory, nutrition, medical, sanitation, and crafting remain canonical.

## 5.2 Preservation data expansion

Extend:
`Assets/StreamingAssets/Data/food_preservation.json`

Add preservation tiers:

### Smoking
Potential fields:
```text
method_id: smoking
facility_requirement
fuel_requirement
food_tags[]
processing_time
yield_ratio
shelf_life_days
quality_modifier
nutrition_retention_profile
contamination_risk_profile
audio_cue_ids[]
localization_keys[]
```

### Pressure canning
Potential fields:
```text
method_id: pressure_canning
facility_requirement
container_requirement
fuel/power_requirement
food_tags[]
processing_time
shelf_life_days
failure_risk_profile
nutrition_retention_profile
audio_cue_ids[]
```

Do not hardcode preservation values in `KitchenNutritionPanel`.

## 5.3 Facilities

Raw target:
- `room_kitchen`
- `room_workshop` upgrades.

Verify actual room IDs and upgrade architecture.

Preferred model:
- smokehouse: construction/facility project with kitchen/workshop prerequisites if design supports combined requirements;
- pressure canner: workshop-crafted device or kitchen upgrade;
- preservation system queries capability tags.

Do not create:
```text
if roomId == "room_kitchen" ...
```
throughout Core.

Use facility tags:
```text
food_smoking
pressure_canning
```

## 5.4 Recipes

Required:
- `recipe_smoke_fish_rations`
- `recipe_can_vegetable_stew`

Validate recipe IDs against current naming.

Targets:
- 90-day emergency shelf life as authored data;
- deterministic output quantity;
- canonical input consumption;
- valid nutrition profile;
- real item IDs.

Potential outputs:
```text
item_smoked_fish_ration
item_canned_vegetable_stew
```
only if actual item schema supports.

If existing generic preserved-ration items already cover them:
- reuse rather than duplicate.

## 5.5 Shelf-life semantics

Shelf life belongs to preservation/cohort authority.

Use:
- production day;
- method;
- storage condition;
- package integrity;
- current degradation.

Do not bake 90 days as a UI label only.

## 5.6 YearOfAsh winter integration

`YearOfAshSystem` owns:
- season;
- severe winter phase;
- onset day;
- external thermal severity.

FoodPreservation receives or queries:
```text
StorageExposureContext
  season
  external_temp_band
  room_temp_band
  insulation_rating
  storage_type
  power_state
```

No duplicate winter clock.

## 5.7 Root-cellar freezing

Raw requirement:
> severe nuclear winter freezes uninsulated root cellars and destroys fresh produce.

Correct model:
1. YearOfAsh enters severe winter.
2. canonical thermal system computes storage room/container exposure.
3. insulation modifies thermal transfer/protection.
4. FoodPreservation evaluates affected fresh-food cohorts.
5. cohort state becomes frozen-damaged/spoiled/lost according to food type.

Do not universally destroy all produce because Day >= 45.

Some foods may tolerate freezing.
Use food tags/profile.

## 5.8 Day 45 preparation

Raw:
> insulate before Day 45 winter onset.

Treat Day 45 as scenario/campaign tuning, not immutable architecture.

Expose:
- forecast/warning event before onset;
- storage readiness UI;
- insulation project.

Materials:
- `scrap_cloth`;
- `insulation_foam`.

Verify exact IDs.

Construction flow:
```text
storage insulation project
→ reserve materials
→ schedule work
→ complete project
→ storage authority increases insulation rating
```

FoodPreservation only consumes resulting storage rating.

## 5.9 Storage lockers

If lockers are item containers:
- canonical storage/container system owns them.

If root cellar is room-level:
- thermal/facility authority owns insulation.

Avoid one-off preservation-local storage DTO.

## 5.10 KitchenNutritionPanel

Enhance presentation with:
- preserved reserve days;
- food-group diversity;
- upcoming deficiency risks;
- seasonal storage vulnerability;
- smoking/canning queue;
- spoilage cohorts;
- winter readiness.

The panel remains presentation-only.

## 5.11 Varied diet and immunity

Raw target:
> meat + greens + preserves bonus for immunity.

Architecture correction:
- nutrition authority computes dietary adequacy/diversity;
- medical/disease system decides immune/illness effects;
- FoodPreservation only supplies preserved food composition.

If a “diet diversity” mechanic exists:
- reuse.

If not:
- introduce as nutrition-layer metric, not preservation metric.

Avoid a generic direct:
`immunity += 20`.

## 5.12 Scurvy

If micronutrients are modeled:
- vitamin C intake/deficit belongs to nutrition authority;
- medical authority instantiates scurvy affliction.

If micronutrients are not modeled:
- do not add a hidden one-off vitamin C counter solely inside preservation.
- either implement a scoped nutrition deficiency system with explicit ADR or defer true scurvy.

## 5.13 Malnutrition

Raw scrap ration exclusivity may cause:
- poor diet quality;
- deficiency;
- stress.

But:
- calorie hunger stays Needs/nutrition;
- deficiency stays nutrition/medical;
- stress stays mental health/Needs.

FoodPreservation emits food intake composition/history only if it is the correct source.

## 5.14 Spoilage odor hazard

Rotten cohorts remain physical inventory/food state.

Flow:
```text
spoiled cohort remains stored
→ FoodPreservation exposes spoilage mass/odor load
→ sanitation/air system consumes odor/biological-waste observation
→ hygiene/disease vector changes
```

No direct:
```text
hygiene -= 50
diseaseChance += X
```
inside FoodPreservation if sanitation already owns those.

Discard action:
- canonical inventory/waste system removes/disposes cohort;
- odor observation updates.

## 5.15 Smokehouse/canning audio

Semantic cues:
- smokehouse crackle;
- bubbling canning vats.

Route through:
`ShelterAcousticDirector`.

Audio:
- driven by active job/facility state;
- no gameplay timing;
- optional cue failure non-fatal;
- headless safe.

## 5.16 Abundant Granary epilogue hook

Expose:
```text
preserved_food_days_at_end
preserved_food_calories
preservation_method_diversity
winter_survival_without_emergency_starvation
storage_loss_events
```

Epilogue decides vignette.

Do not directly set:
`abundant_granary = true`
inside FoodPreservation unless the epilogue contract explicitly uses a derived tag.

## 5.17 Determinism

Spoilage calculation:
- integer/fixed-point preferred;
- deterministic cohort ordering;
- same seed + same environment = same state.

If spoilage is deterministic physics-like degradation:
- no RNG needed.

If contamination/failure uses RNG:
- named seeded stream.

## 5.18 50+ cohort save roundtrip

Build fixture with:
- fresh produce;
- smoked fish;
- canned stew;
- near-expiry food;
- frozen-damaged produce;
- spoiled cohort;
- insulated/uninsulated storage;
- mixed production days.

Assert:
- exact cohort IDs;
- exact remaining shelf life/degradation;
- no merge/split duplication;
- stable normalized hash.

## 5.19 Test

Create:
`FoodPreservationSeasonalWinterTests.cs`.

Cover:
- smoking recipe;
- canning recipe;
- 90-day authored shelf life;
- missing facility;
- missing container/material;
- winter onset;
- insulated cellar;
- uninsulated cellar;
- freeze-tolerant food;
- freeze-sensitive produce;
- save before/after winter transition;
- spoilage odor handoff;
- nutrition presentation;
- scurvy/malnutrition handoff if implemented;
- audio event;
- epilogue metrics.

Update:
`docs/FOOD_PRESERVATION_AUTHORITY_MAP.md`.

---

# 6. Task 8 — Dynamic Campaign Epilogue Branching & Legacy Timeline Chronicle

## 6.1 Mission

Expand `CampaignEpilogueEngine` into a deterministic final read-model over the campaign’s canonical history, with richer vignette categories, a documented composite legacy metric, timeline presentation, survivor epitaphs, and bounded New Game Plus inheritance.

It must remain:
- Core-only;
- engine-reference free;
- side-effect free with respect to completed campaign state.

## 6.2 Twelve new vignettes

Extend:
`Assets/StreamingAssets/Data/campaign_epilogues.json`.

Add 12 new narrative vignettes across:
- Environmental Recovery;
- Faction Sovereignty;
- Cultural Memory;
- Ethical Legacy.

Keep existing vignettes, resulting in the user-requested 16 trigger combinations/test surface only if current baseline has four existing vignette families. Verify actual count.

Suggested 12 concepts:

### Environmental Recovery
1. **Abundant Granary**
2. **Green Below the Ash**
3. **Heat Beneath the Ruins**

### Faction Sovereignty
4. **Lords of the Highway**
5. **Concord of Holdfasts**
6. **The Iron Border**

### Cultural Memory
7. **Technological Golden Age**
8. **The Archive Keepers**
9. **The Sanity of the Deep**

### Ethical Legacy
10. **The Oath Kept**
11. **The Open Hand**
12. **Order at Any Cost**

These are candidate titles; final writing/localization must match project tone.

## 6.3 Vignette schema

Recommended:

```text
vignette_id
category
priority
mutual_exclusion_group optional
requires_all[]
requires_any[]
forbids[]
score_min optional
score_max optional
timeline_tags[]
epilogue_fact_requirements[]
title_key
body_key
slide_art_key
audio_theme_key optional
ng_plus_reward_id optional
```

Predicates should reference stable epilogue facts, not runtime code expressions embedded in JSON.

## 6.4 Epilogue fact projection

Create/reuse:
`CampaignLegacySnapshot`.

Suggested:

```text
CampaignLegacySnapshot
  survivors_alive
  survivors_total_known
  knowledge_saved_score
  master_blueprints
  codex_entries
  ethical_action_summary
  governance_summary
  faction_summary
  environment_summary
  vehicle_fleet_summary
  mental_health_summary
  food_reserve_summary
  rehabilitation_summary
  prisoner_exchange_summary
  major_crisis_history
  milestone_events[]
```

This is a derived snapshot, not a save authority.

## 6.5 Ethical choices

Raw:
- summary executions vs rehabilitation;
- autocracy vs council democracy.

Use canonical history:
- MoralChoiceSystem / execution events;
- prisoner rehabilitation;
- prisoner exchange;
- governance/Plan 43 or current government authority.

Do not infer “autocracy” from one choice.

Build typed metrics:
```text
execution_count
rehabilitation_count
prisoner_exchange_count
coercive_policy_days
council_governance_days
rights_violation_events
mercy/restitution events
```

Only include facts the repo genuinely records.

## 6.6 Shelter Legacy Rating 0..1000

Derived metric.

Recommended high-level composition:

```text
survival_component        0..300
knowledge_component       0..250
ethical_component         0..250
resilience_component      0..120
community_component       0..80
--------------------------------
total                     0..1000
```

Weights are candidates and must be tested.

### Survival
Do not reward only raw headcount.
Consider:
- survival ratio;
- avoidable vs unavoidable losses if inferable;
- stable end-state.

### Knowledge
- Codex preservation;
- master blueprints;
- research;
- archives.

### Ethical
- rehabilitation;
- prisoner exchange;
- governance;
- executions/abuse where canonical.

### Resilience
- winter reserves;
- power;
- shelter survival;
- vehicle logistics.

### Community
- mental-health recovery;
- faction cooperation;
- citizenship integration;
- cultural preservation.

## 6.7 Rating anti-double-count

Do not count the same event across multiple categories without explicit cap.

Example:
- rehabilitated captive becoming citizen could affect:
  - ethical;
  - community.
Use bounded contribution.

Publish formula:
`docs/EPILOGUE_METRIC_AUTHORITY_MAP.md`.

## 6.8 Legacy labels

Avoid presenting 712 vs 713 as dramatically different.

Use bands:
```text
0–199   Ruinous
200–399 Fractured
400–599 Enduring
600–799 Rebuilding
800–1000 Transformative
```

Names/tuning subject to narrative review.

## 6.9 Timeline chart

`CampaignEpilogueModal` presentation uses canonical milestone events.

Timeline facts:
- day;
- event ID;
- category;
- title key;
- outcome key;
- related survivors/factions/locations;
- severity;
- icon/art key.

Examples:
- first winter onset;
- major raid;
- council formation;
- prisoner exchange;
- blueprint breakthrough;
- caravan disaster/success;
- shelter fire;
- major death;
- citizenship oath;
- food crisis resolved.

UI must not reconstruct history by scraping journal prose.

## 6.10 Timeline priority

Too many events can overwhelm.

Use:
- milestone severity;
- category quotas;
- dedupe by source event;
- max visible entries;
- expandable full chronicle if supported.

No duplicate:
- raid event + journal derivative + epilogue derivative
as three separate pivots.

## 6.11 Procedural epitaphs

Deterministic template system.

Inputs:
```text
survivor_id
alive/dead
role history
major relationships
major achievements
major injuries/trauma/recovery
citizenship origin
faction/quest contributions
final status
```

Output:
- localized template ID;
- token values;
- stable variant selected by deterministic hash.

No uncontrolled LLM/network generation.

If survivor is alive:
- this is a “post-war life note,” not an epitaph.
Use appropriate label.

## 6.12 New Game Plus legacy seed

Generate a compact versioned package:

```text
LegacySeedPackage
  schema_version
  source_campaign_seed
  source_campaign_hash
  legacy_rating_band
  unlocked_legacy_perks[]
  inherited_codex_tags[]
  cosmetic_unlocks[]
  bounded_starting_bonus_ids[]
  narrative_legacy_tags[]
  checksum
```

Do not include:
- prior survivor objects;
- full inventory;
- full faction state;
- old quest state;
- old RNG stream.

## 6.13 NG+ starting bonuses

Keep bounded.

Examples:
- one cosmetic shelter emblem;
- small starting recipe familiarity;
- one archive lead;
- modest vehicle repair knowledge;
- limited starting resource pack;
- narrative heritage tag.

Do not carry:
- all advanced tech;
- huge inventory;
- fully repaired fleet;
unless explicit game mode.

Balance:
- NG+ should alter opening texture, not erase survival challenge.

## 6.14 Seed sharing

If “legacy seed” is shareable:
- deterministic compact string/JSON;
- version;
- checksum;
- no private account/user data;
- input validation;
- incompatible version handling.

A shared legacy package should not allow arbitrary item/tech injection.
Whitelist reward IDs.

## 6.15 Lords of the Highway

Read from `VehicleGarageSystem` end-state projection:
- operational overland vehicle count;
- serviceability;
- long-distance mission history;
- route reach.

Do not count wrecks or duplicate garage entries.

## 6.16 The Sanity of the Deep

Raw:
> low average shelter trauma.

Use better mental-health projection:
```text
survivor_count
active_unresolved_trauma_severity
acute_crisis_count
recovery_completion_rate
high_stress_survivor_count
```

Avoid simplistic:
`average trauma token count`.

A shelter can have many resolved traumas and still represent successful recovery.

## 6.17 Technological Golden Age

Require:
- blueprint/research facts;
- actual advanced technology deployment if intended;
- no direct flag from Task 5.

Potential:
- >= N master blueprints;
- >= N advanced recipes/projects used;
- stable power/food state;
- sufficient survivors.

## 6.18 Abundant Granary

Read Task 7 reserve projection.
Potential criteria:
- preserved food reserve days;
- method diversity;
- survived severe winter;
- no active mass starvation.

## 6.19 Epilogue audio

Core emits:
```text
EpilogueAudioTheme
  vinyl_layer_tag
  machinery_layer_tag
  surface_wind_layer_tag
  intensity_band
```

Godot presentation:
- mixes layered crescendo;
- uses `ShelterAcousticDirector`/audio runtime;
- headless Core has zero audio engine refs.

Music selection:
- can reflect turntable/vinyl legacy if canonical album known;
- do not persist runtime bus state in epilogue snapshot.

## 6.20 Deterministic selection

Selection:
1. evaluate canonical snapshot;
2. collect eligible vignettes;
3. sort by category/priority/stable ID;
4. apply mutual exclusion;
5. apply maximum count/category quotas;
6. choose deterministic variant.

No RNG required unless authored tie-break variety; if used, seed from final campaign hash.

## 6.21 16-combination tests

Create:
`CampaignEpilogueBranchingTests.cs`.

Verify actual intended 16 scenario combinations rather than only 16 vignettes if current total differs.

Scenarios should vary:
- high/low survival;
- high/low knowledge;
- rehabilitation/execution;
- council/autocracy;
- strong/weak faction sovereignty;
- fleet/no fleet;
- strong/poor mental-health recovery;
- abundant/starved reserves;
- high/low technology.

Assertions:
- eligible vignettes;
- exclusion;
- priority;
- legacy rating;
- timeline;
- survivor life notes;
- NG+ reward package;
- deterministic order.

## 6.22 Engine independence

Source scan `CampaignEpilogueEngine` and Core dependencies:
- zero `Godot`;
- zero `UnityEngine`;
- zero UI node types;
- zero audio server calls.

Update:
`docs/EPILOGUE_METRIC_AUTHORITY_MAP.md`.

---

# 7. Cross-System Integration Map

```text
PREWAR ARCHIVES
    │
    ├── chemicals/inventory
    ├── specialist schedule/skills
    ├── Codex
    ├── ResearchSystem
    ├── TechTree
    ├── Foundry/Workshop/Construction
    ├── faction information/trade
    └── epilogue technology facts
    │
    ▼
CAPTIVE SYSTEM
    │
    ├── custody/security
    ├── vocational skill progression
    ├── parole/citizenship
    ├── faction prisoner exchange
    ├── mental-health recovery
    └── ethical epilogue facts
    │
    ▼
SEASONAL FOOD
    │
    ├── YearOfAsh
    ├── storage thermal/insulation
    ├── recipes/inventory
    ├── nutrition/medical
    ├── sanitation/air
    └── epilogue resilience facts
    │
    ▼
CAMPAIGN EPILOGUE
        ├── derives facts
        ├── scores legacy
        ├── selects vignettes
        ├── builds timeline
        ├── builds survivor life notes
        └── generates bounded NG+ package
```

The epilogue does not write back into the completed campaign.

---

# 8. Blueprint Unlock Transaction

Exactly-once sequence:

```text
archive stage completion
→ stable `blueprint_discovered:<archive>:<blueprint>` event
→ ResearchSystem consumes
→ TechTree consumes
→ recipe/project eligibility changes
→ notification
→ Codex entry if configured
→ epilogue history records semantic milestone
```

Processed event IDs prevent:
- save/reload duplicate unlock;
- repeated notification;
- repeated faction-reaction trigger.

---

# 9. Faction Information Boundary

Faction response requires information provenance.

Information states:
```text
UNKNOWN
RUMORED
SUSPECTED
CONFIRMED
DISCLOSED
TRADED
```

Plan 131 or current intel authority owns propagation.

Possible sources:
- spy;
- envoy;
- direct trade;
- public construction using tech;
- captured copy;
- leaked research.

Archive system publishes facts.
It does not assign omniscience.

---

# 10. Archive Copy/Trade Boundary

Define:
- original physical spool;
- restored master;
- data transcription;
- blueprint copy;
- knowledge license.

Only an actual copy artifact or information-transfer action can be traded repeatedly.

If copy creation exists:
```text
source archive
+ blank medium/resources
+ time
+ specialist
→ copy item/info artifact
```

Copy quality/provenance may affect price.

Trade:
- canonical economy sets value;
- faction reputation applies consequence;
- treaty system grants perks.

---

# 11. Captive Conspiracy Risk Projection

Suggested diagnostic projection:

```text
CaptiveConspiracyRiskContext
  detention_days
  treatment_quality
  security_score
  guard_coverage
  guard_negligence
  crowding
  tool_access
  social_contact
  faction_loyalty
  rehabilitation_progress
  failed_escape_memory
```

Normalize to deterministic risk band.

Do not expose exact hidden risk to player unless surveillance/intel systems justify it.

UI can show:
- secure;
- uneasy;
- unstable;
- critical
based on known information.

---

# 12. Oath/Citizenship Atomicity

Preconditions:
- captive exists;
- eligible;
- alive;
- not in active exchange/escape/combat;
- oath accepted;
- citizenship capacity valid.

Commit:
1. lock transition;
2. detach captive-only jobs;
3. clear detention reservation;
4. canonical citizen/survivor status set;
5. preserve same person ID;
6. migrate permissible relationships/skills/inventory refs;
7. remove captive-state membership;
8. emit one citizenship event.

Invariant:
```text
is_captive XOR is_citizen
```
after completion.

Rollback on failure.

---

# 13. Winter Storage Exposure Model

Food preservation should query, not own:

```text
external season/weather
→ shelter thermal model
→ room/storage temperature
→ insulation effect
→ FoodPreservation cohort response
```

Food cohort response:
- safe;
- frozen;
- freeze-damaged;
- accelerated spoilage;
- preserved.

Not every frozen food is destroyed.

---

# 14. Nutrition/Medical Boundary

Food intake facts:
```text
calories
protein
food_group_tags
micronutrient_tags if supported
preservation method
quality
```

Nutrition authority:
- adequacy;
- diversity;
- deficiency risk.

Medical:
- scurvy;
- malnutrition;
- related afflictions.

Mental health:
- stress consequence if nutrition/illness emits appropriate event.

No direct `immunity` field in FoodPreservation unless canonical nutrition model already owns it.

---

# 15. Spoilage/Sanitation Boundary

```text
spoiled cohort mass
→ biological waste/odor observation
→ sanitation/air system
→ hygiene/air quality
→ disease/comfort consumers
```

One consequence path only.

Discard:
- removes cohort through inventory/waste;
- clears observation next update.

---

# 16. Legacy Rating Formula — Candidate v1

A fully documented candidate:

```text
SURVIVAL 300
  end_survival_ratio                      0..180
  dependents/vulnerable_survivor outcome 0..40
  catastrophic-loss avoidance            0..40
  stable shelter end-state               0..40

KNOWLEDGE 250
  codex preservation                     0..60
  master blueprint reconstruction        0..90
  research completion                    0..50
  advanced tech deployment               0..50

ETHICS 250
  rehabilitation/citizenship             0..70
  prisoner exchange/mercy                0..50
  governance participation               0..60
  rights/abuse penalties                  -0..100 within bounded component
  restitution/peace outcomes              0..70

RESILIENCE 120
  winter food reserve                     0..35
  power/thermal resilience                0..25
  vehicle logistics                       0..25
  crisis recovery                         0..35

COMMUNITY 80
  mental-health recovery                  0..30
  faction cooperation                     0..20
  cultural memory                         0..20
  social/citizenship integration          0..10
```

Final:
```text
clamp(components, 0..max)
sum
clamp(0..1000)
```

This is a candidate pending current system coverage.

If a fact is unavailable:
- do not silently assume ideal/zero without design decision.
- either remove/reweight component or add an explicit neutral handling rule.

---

# 17. Epilogue Trigger Priority

Recommended precedence:
1. catastrophic/ending-defining mutually exclusive state;
2. governance/sovereignty;
3. ethical legacy;
4. technology/cultural legacy;
5. environmental/resilience;
6. specialist achievements.

Use category quotas so one campaign does not render 12 near-duplicate positive slides.

Stable tie-break:
- priority;
- vignette ID.

---

# 18. New Game Plus Security & Validation

Legacy package parser must:
- validate schema version;
- validate checksum;
- validate every perk/reward ID against whitelist;
- reject unknown IDs;
- cap quantities;
- reject duplicate perks;
- reject unsupported future schema cleanly.

Never trust a shared legacy seed as arbitrary serialized game state.

Optional:
- mark imported package as external/shared for analytics only, not gameplay difference.

---

# 19. Persistence Matrix

| Fact | Owner | Persist? | Integration rule |
|---|---|---:|---|
| archive cleaning/progress | PrewarArchiveDecryption | yes | canonical |
| blueprint unlocked | Research/TechTree | yes | event exactly once |
| recipe availability | recipe authority | derived from tech | no duplicate archive flag |
| Codex known entry | Codex authority | yes | archive emits unlock |
| physical archive/copy | inventory/archive item | yes | provenance |
| faction archive knowledge | intel/faction | yes if canonical | no archive duplicate |
| captive custody | PrisonerSystem | yes | canonical |
| cabal | PrisonerSystem | yes | canonical |
| vocational progress | SkillProgression + prisoner linkage | split | no duplicate skills |
| citizenship | survivor/citizenship authority | yes | captive removed |
| exchange outcome | faction/diplomacy | yes | semantic event |
| food cohort | FoodPreservation | yes | canonical |
| storage temperature | thermal | yes/derived | not food save |
| insulation | storage/build | yes | not food save |
| nutrition deficiency | nutrition/medical | yes | not food save |
| spoilage odor effect | sanitation/air | derived/state | no duplicate |
| epilogue legacy score | derived | no need in campaign save | recompute |
| timeline | derived from semantic history | no duplicate |
| epitaph text | derived | no raw prose save |
| NG+ package | separate export/unlock store | yes | compact/versioned |

---

# 20. Restore Order

Recommended:
1. catalogs/localization;
2. survivor/person identity;
3. items/inventory;
4. rooms/build/storage;
5. thermal/YearOfAsh;
6. skills/schedule;
7. prisoner/captive;
8. mental health/relations;
9. food preservation/nutrition/medical;
10. archive/decryption;
11. Research/TechTree/recipes/projects;
12. faction/intelligence/trade;
13. journal/Codex/history;
14. derived epilogue services;
15. UI/presentation/audio.

After restore:
- reconciliation may repair references;
- it must not replay unlock/trade/oath/spoilage consequences.

---

# 21. Determinism

## Archive
Named seed stream:
```text
archive-decryption:<archive-id>:<stage>:<day>
```

## Captive
```text
captive-conspiracy:<cabal-or-group>:<day>
captive-oath:<person>:<milestone>
```

## Food
Prefer deterministic degradation.
If stochastic contamination:
```text
food-preservation:<cohort>:<day>
```

## Epilogue
Prefer no RNG.
If template variation:
```text
final_campaign_hash + survivor_id + vignette_id
```

Same canonical campaign state:
- same score;
- same vignettes;
- same ordering;
- same timeline selection;
- same survivor life-note variants;
- same NG+ package.

---

# 22. Failure Injection Matrix

## F45.1 Archive code directly toggles recipe JSON runtime state
Expected: recipe authority gate fails.

## F45.2 Blueprint unlock repeats after restore
Expected: exactly-once gate fails.

## F45.3 Faction envoy appears immediately despite no information path
Expected: information-provenance gate fails.

## F45.4 Same unique archive sold repeatedly without copy
Expected: provenance/trade gate fails.

## F45.5 Engineer remains on guard duty while decoding
Expected: schedule exclusivity fails.

## F45.6 chemical consumed despite failed cleaning validation
Expected: inventory transaction fails.

## F45.7 Day 31 automatically creates cabal regardless conditions
Expected: conspiracy semantics gate fails.

## F45.8 security score copied into prisoner save and goes stale
Expected: security authority gate fails.

## F45.9 escape creates virtual workshop tool
Expected: inventory authority fails.

## F45.10 captive remains in prisoner list after citizenship
Expected: atomic transition fails.

## F45.11 Oath creates duplicate survivor ID
Expected: identity gate fails.

## F45.12 prisoner exchange changes faction goodwill twice
Expected: exactly-once diplomacy gate fails.

## F45.13 unrest whisper audio reveals undetected cabal
Expected: information/audio gate fails.

## F45.14 FoodPreservation destroys all produce just because day >=45
Expected: thermal boundary fails.

## F45.15 insulation state copied into food cohorts
Expected: storage authority fails.

## F45.16 Kitchen panel applies immunity buff itself
Expected: nutrition/medical authority fails.

## F45.17 FoodPreservation directly creates scurvy
Expected: medical authority fails.

## F45.18 rotten food directly decrements hygiene while Sanitation also consumes odor
Expected: duplicate hygiene effect fails.

## F45.19 50-cohort restore merges distinct batches incorrectly
Expected: cohort identity gate fails.

## F45.20 epilogue engine stores a running moral score during campaign
Expected: derived-metric gate fails.

## F45.21 average trauma token count triggers Sanity vignette despite severe active crises
Expected: mental-health metric semantics fail.

## F45.22 Golden Age unlocks merely from possessing encrypted spool
Expected: technology adoption predicate fails.

## F45.23 NG+ package contains full prior inventory
Expected: bounded inheritance gate fails.

## F45.24 shared legacy seed injects unknown reward IDs
Expected: package whitelist fails.

## F45.25 epitaph differs because dictionary iteration order changed
Expected: deterministic template gate fails.

## F45.26 `CampaignEpilogueEngine` references Godot Control
Expected: Core engine-independence gate fails.

---

# 23. Performance Budgets

## Archive
- decryption update O(active archives + assigned specialists);
- no full recipe-catalog scan every effort tick;
- unlock predicates indexed/cached;
- Codex fragments event-driven.

## Captives
- cabal evaluation at day/event boundary;
- no all-captives pairwise O(n²) every frame;
- social compatibility can be indexed/grouped;
- active escape state only updated when relevant.

## Food
- 50–500 cohorts supported;
- degradation day-boundary/batched;
- storage exposure grouped by container/room;
- no per-frame cohort loop;
- save size monitored.

## Epilogue
- one-shot final projection;
- vignette selection O(vignettes + facts);
- timeline sorting bounded;
- survivor life-note generation O(survivors);
- no frame-loop Core evaluation.

---

# 24. Content Acceptance

## Blueprint archive ladder
```text
DISCOVERED
LOADED
REGISTERED
CLEANING_REACHABLE
SPECIALIST_REQUIREMENTS_REACHABLE
DECRYPTABLE
BLUEPRINT_EVENT_EMITTED
TECH_CONSUMER_REACHED
RECIPE/PROJECT_CONSUMER_REACHED
CODEX_CONSUMER_REACHED
EPILOGUE_FACT_REACHED
```

## Captive rehabilitation ladder
```text
PROFILE_LOADED
CAPTIVE_REACHED
CUSTODY_STATE_REACHED
VOCATIONAL_OPTION_REACHED
MENTOR_REACHED
SKILL_PROGRESS_OBSERVED
PAROLE/OATH_ELIGIBILITY_REACHED
CITIZEN_TRANSITION_OBSERVED
```

## Conspiracy ladder
```text
RISK_SOURCE_REACHED
CABAL_FORMED
PLAN_ADVANCED
TOOL/SECURITY_ACTION_REACHED
ESCAPE_RESOLVED
SAVE_ROUNDTRIP
```

## Food ladder
```text
METHOD_LOADED
FACILITY_REACHED
RECIPE_CRAFTED
COHORT_CREATED
STORAGE_EXPOSURE_REACHED
WINTER_EFFECT_REACHED
NUTRITION_CONSUMER_REACHED
SANITATION_CONSUMER_REACHED
EPILOGUE_FACT_REACHED
```

## Epilogue ladder
```text
VIGNETTE_LOADED
FACT_PREDICATE_RESOLVED
ELIGIBLE
PRIORITIZED
SELECTED
LOCALIZED
TIMELINE_PRESENTED
AUDIO_THEME_PRESENTED
NGPLUS_REWARD_VALIDATED
```

No dead content accepted.

---

# 25. Accessibility & Localization

## Archives
- blueprint titles/descriptions localized;
- cipher stage labels;
- chemical prerequisite reasons;
- specialist requirements textual;
- unlock notification accessible;
- Codex reader text scaling.

## Captives
- conspiracy/rehabilitation UI avoids dehumanizing shorthand where richer labels exist;
- custody condition warnings textual;
- oath/exchange consequences explicit;
- dialogue localized;
- hidden conspiracy state not exposed accidentally.

## Food
- shelf life shown textually;
- winter-vulnerability state not color-only;
- diet deficiency warnings accessible;
- preserved-food method icons have labels.

## Epilogue
- timeline usable keyboard/controller;
- chart has list/table equivalent;
- no information only in color/graph;
- epitaph/life-note text scalable;
- audio crescendo has subtitles/text-independent narrative;
- NG+ perks explained in text.

All new vignette, archive, prisoner-dialogue, food, recipe, and Codex keys must pass localization integrity.

---

# 26. Test Strategy

## 26.1 `PrewarArchiveTechUnlockTests.cs`
- 8 master archives;
- 3 chemical cleaning inputs;
- cleaning atomicity;
- Engineer-only insufficient where Scholar required;
- Scholar-only insufficient;
- valid multi-specialist effort;
- seeded effort;
- tech unlock once;
- foundry recipe unlock;
- workshop recipe unlock;
- construction project unlock;
- Codex fragment;
- no faction reaction without knowledge;
- reaction after legitimate knowledge propagation;
- copy/trade provenance;
- save/load;
- epilogue technology projection.

## 26.2 `CaptiveRehabilitationAndEscapeTests.cs`
- 60-day custody;
- secure/humane baseline;
- harsh low-security;
- negligent guards;
- cabal formation;
- tool theft;
- breach;
- surface rush;
- recapture;
- escape;
- vocational apprentice;
- skill progress;
- parole;
- peer support;
- oath;
- citizenship atomicity;
- officer exchange;
- refusal;
- deterministic rolls;
- save/load.

## 26.3 `FoodPreservationSeasonalWinterTests.cs`
- smoking;
- canning;
- 90-day shelf life;
- facility prerequisites;
- winter phase;
- insulation;
- freeze-sensitive produce;
- freeze-tolerant cohort;
- spoilage odor;
- discard;
- nutrition handoff;
- deficiency/medical handoff;
- 50+ cohort roundtrip;
- deterministic hash;
- epilogue reserve projection.

## 26.4 `CampaignEpilogueBranchingTests.cs`
- all authored vignette predicates;
- 16 requested composite scenario combinations;
- deterministic priority;
- mutual exclusions;
- legacy rating boundaries;
- ethics/governance variations;
- vehicle fleet;
- mental-health recovery;
- food reserve;
- master tech;
- timeline dedupe;
- survivor life-note determinism;
- NG+ package;
- invalid legacy seed;
- engine-reference source scan.

---

# 27. Recommended Commit Breakdown

## Task 5 — Master Blueprints
```text
45A-01 authority audit/ADRs
45A-02 eight archive definitions
45A-03 cleaning-stage schema
45A-04 chemical transaction
45A-05 specialist-slot schema
45A-06 assignment bridge
45A-07 deterministic effort model
45A-08 blueprint semantic event
45A-09 Research/TechTree consumers
45A-10 foundry/workshop unlock predicates
45A-11 construction-project unlocks
45A-12 Codex fragment bridge
45A-13 faction information propagation
45A-14 archive copy/trade provenance
45A-15 envoy/theft hooks
45A-16 acoustic fanfare
45A-17 epilogue technology facts
45A-18 tests/integrity/docs
```

## Task 6 — Captives
```text
45B-01 captive profile schema
45B-02 conspiracy-risk projection
45B-03 cabal state
45B-04 daily/event formation
45B-05 escape state machine
45B-06 tool theft
45B-07 cell security bridge
45B-08 surface transition
45B-09 vocational tracks
45B-10 mentor/schedule integration
45B-11 SkillProgression handoff
45B-12 parole/oath eligibility
45B-13 captive→citizen transaction
45B-14 peer-support mental-health bridge
45B-15 officer exchange
45B-16 faction goodwill/hostage return
45B-17 unrest audio
45B-18 save compatibility
45B-19 60-day tests
45B-20 authority-map docs
```

## Task 7 — Food
```text
45C-01 preservation authority audit
45C-02 smoking schema
45C-03 canning schema
45C-04 facility capability tags
45C-05 smoked fish recipe
45C-06 canned vegetable stew recipe
45C-07 90-day cohort semantics
45C-08 YearOfAsh exposure bridge
45C-09 storage thermal adapter
45C-10 insulation project/materials
45C-11 winter warning/readiness
45C-12 KitchenNutritionPanel projection
45C-13 diet diversity authority bridge
45C-14 scurvy/malnutrition disposition
45C-15 spoilage odor sanitation bridge
45C-16 audio cues
45C-17 epilogue reserve facts
45C-18 50+ cohort save/determinism
45C-19 tests/integrity/docs
```

## Task 8 — Epilogue
```text
45D-01 epilogue authority audit/ADR
45D-02 12 vignette definitions
45D-03 fact projection snapshot
45D-04 ethical/governance metrics
45D-05 legacy rating formula
45D-06 anti-double-count normalization
45D-07 vignette priority/exclusion
45D-08 timeline milestone projection
45D-09 timeline UI contract
45D-10 survivor life-note templates
45D-11 deterministic variant selector
45D-12 NG+ package schema
45D-13 NG+ reward whitelist
45D-14 seed sharing/checksum
45D-15 Lords of Highway predicate
45D-16 Sanity of Deep predicate
45D-17 Golden Age predicate
45D-18 Abundant Granary predicate
45D-19 epilogue audio theme
45D-20 branching tests
45D-21 engine-ref gate
45D-22 metric authority docs
```

## Final hardening
```text
45E-01 restore-order audit
45E-02 cross-system exactly-once audit
45E-03 content reachability
45E-04 localization integrity
45E-05 deterministic fingerprints
45E-06 long-horizon 60/120/180-day sims
45E-07 performance/state-size
45E-08 full Core tests
45E-09 verify-fast
45E-10 SHIP/NO-SHIP report
```

---

# 28. Risk Register

## R45.1 Archive becomes recipe authority
Mitigation: blueprint event → Research/TechTree → recipe predicates.

## R45.2 Factions become omniscient
Mitigation: information provenance gate.

## R45.3 Unique archive sold infinitely
Mitigation: physical copy/provenance contract.

## R45.4 specialist assignment duplicates schedule
Mitigation: one scheduler.

## R45.5 cabal formation is simplistic timer
Mitigation: multi-input custody/security risk projection.

## R45.6 captivity/citizenship overlap
Mitigation: atomic XOR transition.

## R45.7 vocational rehabilitation becomes exploitative free labor bonus
Mitigation: real schedule, mentor, skill progression, governance.

## R45.8 audio leaks hidden cabal
Mitigation: information-aware acoustic cues.

## R45.9 food owns temperature
Mitigation: YearOfAsh + thermal/storage observation.

## R45.10 scurvy added as one-off hidden counter
Mitigation: nutrition/medical authority or defer.

## R45.11 spoilage penalties double
Mitigation: odor observation → sanitation/air only.

## R45.12 old food cohorts corrupt on new preservation tiers
Mitigation: versioned migration + 50+ cohort fixtures.

## R45.13 Legacy Rating becomes arbitrary moral truth
Mitigation: transparent derived components from recorded facts.

## R45.14 one action counted in many legacy components
Mitigation: contribution caps and anti-double-count matrix.

## R45.15 NG+ trivializes survival
Mitigation: bounded bonuses and whitelist.

## R45.16 procedural epitaph nondeterminism
Mitigation: localized templates + stable hash selection.

---

# 29. Exhaustive Acceptance Checklist

## Task 5 — Archive/Blueprints
- [ ] archive authority audited
- [ ] ResearchSystem audited
- [ ] TechTree audited
- [ ] foundry recipe authority audited
- [ ] workshop recipe authority audited
- [ ] construction project authority audited
- [ ] Codex authority audited
- [ ] faction information authority audited
- [ ] trade/reputation/treaty authority audited
- [ ] 8 master archive definitions
- [ ] Geothermal Heat Exchangers
- [ ] Hydroponic Nutrient Synthesis
- [ ] Magnetic Rail Propulsion
- [ ] Pulse Radars
- [ ] four additional consumer-backed master technologies
- [ ] archive IDs unique
- [ ] localization complete
- [ ] chemicals ID verified
- [ ] purified_alcohol ID verified
- [ ] acid_neutralizer ID verified
- [ ] cleaning stages data-driven
- [ ] cleaning atomic
- [ ] no chemical loss on failed validation
- [ ] Engineer requirement canonical
- [ ] Scholar requirement canonical
- [ ] multiple specialists supported
- [ ] specialist schedule exclusivity
- [ ] specialist skill not duplicated
- [ ] deterministic effort points
- [ ] named RNG stream if uncertainty used
- [ ] no wall clock
- [ ] blueprint event stable
- [ ] blueprint event exactly once
- [ ] ResearchSystem consumer
- [ ] TechTree consumer
- [ ] foundry recipe unlock predicate
- [ ] workshop recipe unlock predicate
- [ ] construction project unlock if authored
- [ ] JSON not mutated at runtime
- [ ] Codex fragment IDs stable
- [ ] Codex text localized
- [ ] no lore prose duplicated in archive save
- [ ] faction reaction requires knowledge
- [ ] no omniscient envoy
- [ ] theft hook uses event/espionage/security
- [ ] archive trade unit defined
- [ ] original/copy provenance
- [ ] no repeated unique-original sale
- [ ] reputation effect canonical
- [ ] treaty perks canonical
- [ ] unlock chime
- [ ] reel cue
- [ ] headless audio safe
- [ ] Golden Age epilogue fact
- [ ] `PrewarArchiveTechUnlockTests.cs`
- [ ] save/load unlocked Codex
- [ ] no unlock replay
- [ ] CatalogIntegrityValidator
- [ ] authority map updated

## Task 6 — Captives
- [ ] captive authority audited
- [ ] security score scale verified
- [ ] guard negligence semantics verified
- [ ] loyalty/trust scale verified
- [ ] psychological profile refs do not duplicate traits
- [ ] conspiracy tags
- [ ] vocational affinity tags
- [ ] dialogue localization
- [ ] cabal state stable IDs
- [ ] detention duration input
- [ ] treatment quality input
- [ ] security input
- [ ] guard coverage input
- [ ] negligence input
- [ ] overcrowding input
- [ ] tool access input
- [ ] social contact input
- [ ] rehabilitation input
- [ ] 30-day threshold treated as tuning
- [ ] <150 security treated as tuning
- [ ] no automatic Day-31 cabal
- [ ] deterministic formation
- [ ] escape state machine
- [ ] organizing stage
- [ ] tool acquisition stage
- [ ] breach stage
- [ ] internal movement
- [ ] surface rush
- [ ] recapture
- [ ] escape
- [ ] surrender/failure where supported
- [ ] workshop tool theft uses inventory
- [ ] no virtual stolen tool
- [ ] security authority resolves breach
- [ ] guard/combat authority resolves interception
- [ ] vocational rehabilitation exists
- [ ] apprentice time consumed
- [ ] mentor time consumed
- [ ] SkillProgression used
- [ ] no duplicate captive skill
- [ ] formal labor-union system only if canonical
- [ ] collective grievance follow-on disposition
- [ ] 800 loyalty threshold verified/reworked
- [ ] oath eligibility data-driven
- [ ] Oath transition atomic
- [ ] same person ID retained
- [ ] captive state removed
- [ ] citizen state canonical
- [ ] no captive+citizen overlap
- [ ] paroled trauma recovery via mental health
- [ ] peer support does not delete trauma directly
- [ ] prisoner exchange choice
- [ ] faction goodwill canonical
- [ ] hostage return canonical
- [ ] refusal does not invent labor bonus
- [ ] bunk banging cue
- [ ] whisper cue
- [ ] hidden cabal not leaked by audio
- [ ] deterministic escape
- [ ] deterministic oath if stochastic
- [ ] save cabal
- [ ] save vocational linkage
- [ ] no duplicate skills/security/mental health
- [ ] 60-day lifecycle test
- [ ] prisoner dialogue integrity
- [ ] authority map updated

## Task 7 — Food
- [ ] preservation authority audited
- [ ] YearOfAsh audited
- [ ] thermal/storage authority audited
- [ ] build/facility authority audited
- [ ] nutrition authority audited
- [ ] medical authority audited
- [ ] sanitation/air authority audited
- [ ] smoking tier
- [ ] canning tier
- [ ] stable method IDs
- [ ] facility requirements
- [ ] fuel/container requirements
- [ ] method localization
- [ ] `recipe_smoke_fish_rations`
- [ ] `recipe_can_vegetable_stew`
- [ ] output item IDs valid
- [ ] 90-day shelf life authored
- [ ] shelf life is simulation state
- [ ] room_kitchen ID verified
- [ ] room_workshop ID verified
- [ ] capability tags preferred
- [ ] YearOfAsh owns winter
- [ ] no duplicated winter clock
- [ ] storage exposure context
- [ ] insulation rating canonical
- [ ] `scrap_cloth` ID verified
- [ ] `insulation_foam` ID verified
- [ ] insulation construction transactional
- [ ] Day 45 treated as scenario/tuning
- [ ] advance winter warning
- [ ] uninsulated root cellar test
- [ ] insulated cellar test
- [ ] freeze-sensitive food
- [ ] freeze-tolerant food
- [ ] no universal frozen=destroyed
- [ ] KitchenNutritionPanel presentation
- [ ] reserve days
- [ ] diet diversity
- [ ] deficiency warning
- [ ] seasonal vulnerability
- [ ] smoking/canning queue
- [ ] immunity not directly mutated by panel
- [ ] nutrition computes diversity
- [ ] scurvy only if micronutrient authority exists
- [ ] malnutrition canonical
- [ ] stress consequence canonical
- [ ] spoiled cohort remains canonical item/cohort
- [ ] odor observation
- [ ] sanitation/air consumes odor
- [ ] no duplicate hygiene decrement
- [ ] discard removes odor source
- [ ] smokehouse crackle cue
- [ ] canning bubbling cue
- [ ] audio non-blocking
- [ ] Abundant Granary metrics
- [ ] deterministic spoilage
- [ ] 50+ cohort roundtrip
- [ ] no cohort merge corruption
- [ ] CatalogIntegrityValidator
- [ ] authority map updated

## Task 8 — Epilogue
- [ ] CampaignEpilogueEngine audited
- [ ] existing vignette count verified
- [ ] 12 new vignette definitions
- [ ] Environmental Recovery category
- [ ] Faction Sovereignty category
- [ ] Cultural Memory category
- [ ] Ethical Legacy category
- [ ] stable vignette IDs
- [ ] priorities
- [ ] mutual exclusion groups
- [ ] localization
- [ ] CampaignLegacySnapshot
- [ ] survivor metrics canonical
- [ ] knowledge metrics canonical
- [ ] ethical metrics canonical
- [ ] governance metrics canonical
- [ ] faction metrics canonical
- [ ] vehicle metrics canonical
- [ ] mental-health metrics canonical
- [ ] food metrics canonical
- [ ] no running legacy score mutation during campaign
- [ ] rating 0..1000
- [ ] survival component documented
- [ ] knowledge component documented
- [ ] ethical component documented
- [ ] resilience component documented
- [ ] community component documented
- [ ] anti-double-count rules
- [ ] unavailable-fact policy
- [ ] legacy bands
- [ ] timeline semantic event source
- [ ] crisis-day entries
- [ ] resolution entries
- [ ] timeline dedupe
- [ ] timeline max/priority
- [ ] chart has list alternative
- [ ] survivor post-war life notes
- [ ] dead survivor epitaph semantics
- [ ] deterministic template selection
- [ ] no network/LLM requirement
- [ ] NG+ package versioned
- [ ] source campaign hash
- [ ] whitelisted perks
- [ ] bounded bonuses
- [ ] no prior full inventory
- [ ] no prior full survivor state
- [ ] seed-sharing checksum
- [ ] incompatible version handling
- [ ] Lords of Highway uses operational fleet
- [ ] Sanity of Deep uses meaningful mental-health projection
- [ ] Golden Age requires technology achievement
- [ ] Abundant Granary uses food reserve facts
- [ ] epilogue audio theme Core-only
- [ ] Godot mix outside Core
- [ ] deterministic selection order
- [ ] 16 requested scenario combinations tested
- [ ] zero Godot refs in Core epilogue
- [ ] zero UnityEngine refs in Core epilogue
- [ ] CatalogIntegrityValidator
- [ ] metric authority map updated

## Shared
- [ ] one item authority
- [ ] one scheduler
- [ ] one skill authority
- [ ] one mental-health authority
- [ ] one faction/diplomacy authority
- [ ] one thermal authority
- [ ] one nutrition/medical path
- [ ] one audio intent authority
- [ ] restore side-effect free
- [ ] stable IDs
- [ ] deterministic event ordering
- [ ] no Guid.NewGuid
- [ ] no wall clock
- [ ] headless safe
- [ ] build passes
- [ ] Core tests pass
- [ ] data integrity passes
- [ ] content acceptance passes
- [ ] verify-fast passes

---

# 30. Long-Horizon Simulation Matrix

## 30-day archive
- one master spool;
- scarce chemicals;
- Engineer but no Scholar;
- expected: blocked multi-discipline stage, no fake progress.

## 60-day archive
- Engineer + Scholar;
- cleaning complete;
- save/reload;
- expected: deterministic blueprint unlock and recipe availability.

## 120-day archive/faction
- player keeps discovery secret;
- expected: no magical envoy.
- later disclose/trade copy;
- expected: faction response.

## 60-day humane captivity
- strong security;
- vocational program;
- expected: low conspiracy, skill progress, possible parole/oath eligibility.

## 60-day harsh captivity
- poor security;
- negligent guards;
- expected: cabal risk and deterministic escape branch.

## 120-day mixed prisoner policy
- exchanges + rehabilitation + refusal;
- expected: coherent faction/ethical history.

## 90-day winter food
- uninsulated cellar;
- fresh produce;
- expected: food-specific freezing consequences.

## 90-day insulated food
- same inventory, insulated storage;
- expected: materially improved survival.

## 180-day preservation
- smoked/canned/fresh cohorts;
- expected: deterministic shelf-life, spoilage, nutrition and sanitation interactions.

## Endgame legacy scenarios
- high technology / low ethics;
- low technology / high ethics;
- high survival / food scarcity;
- fleet logistics;
- strong rehabilitation;
- authoritarian security;
- council democracy;
- high trauma but strong recovery;
- low unresolved trauma;
- Golden Age;
- Abundant Granary;
- Lords of Highway.

No vignette should contradict canonical facts.

---

# 31. Verification Commands

Use current equivalents:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj

dotnet test Ashfall.Core.Tests --filter PrewarArchiveTechUnlockTests
dotnet test Ashfall.Core.Tests --filter CaptiveRehabilitationAndEscapeTests
dotnet test Ashfall.Core.Tests --filter FoodPreservationSeasonalWinterTests
dotnet test Ashfall.Core.Tests --filter CampaignEpilogueBranchingTests

godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --content-utilization-selftest
godot --headless --path . -- --bridge-selftest
godot --headless --path . -- --real-campaign-journey-selftest

bash scripts/ci/content-acceptance-gate.sh
bash scripts/ci/verify-fast.sh
```

Add source scan:
```bash
rg -n "UnityEngine|Godot" Assets/Ashfall.Core src/Core
```
scoped to actual Core locations.

Record:
- commit;
- seed;
- exact command;
- result;
- state hash for deterministic fixtures.

---

# 32. SHIP / NO-SHIP Gate

**SHIP** only if:

```text
archive_decryption_authorities == 1
AND research_authorities == 1
AND tech_tree_authorities == 1
AND foundry_recipe_authorities == 1
AND workshop_recipe_authorities == 1
AND construction_project_authorities == 1
AND codex_authorities == 1
AND faction_information_authorities == 1
AND trade_reputation_authorities == 1

AND captive_status_authorities == 1
AND security_authorities == 1
AND skill_progression_authorities == 1
AND citizenship_authorities == 1
AND diplomacy_authorities == 1
AND mental_health_authorities == 1

AND food_preservation_authorities == 1
AND year_of_ash_authorities == 1
AND thermal_storage_authorities == 1
AND nutrition_authorities == 1
AND medical_affliction_authorities == 1
AND sanitation_air_authorities == 1

AND epilogue_fact_authorities_are_read_only == true

AND runtime_json_recipe_mutations == 0
AND duplicate_blueprint_unlocks == 0
AND omniscient_faction_archive_reactions == 0
AND repeated_unique_archive_sales_without_copy == 0
AND chemical_failed_action_consumptions == 0
AND specialist_schedule_double_bookings == 0

AND automatic_day31_cabal_creation == 0
AND duplicate_security_state_in_prisoner_save == 0
AND virtual_stolen_tools == 0
AND captive_citizen_overlap == 0
AND duplicate_person_ids_from_oath == 0
AND duplicate_exchange_faction_effects == 0
AND hidden_cabal_audio_information_leaks == 0

AND duplicate_winter_temperature_state_in_food_system == 0
AND day45_unconditional_food_destruction == 0
AND duplicate_storage_insulation_state == 0
AND kitchen_ui_direct_immunity_mutations == 0
AND food_system_direct_scurvy_mutations == 0
AND duplicated_spoilage_hygiene_penalties == 0
AND food_cohort_restore_corruption == 0

AND running_campaign_legacy_score_state == 0
AND epilogue_noncanonical_fact_inference == 0
AND legacy_component_unbounded_double_counts == 0
AND ngplus_full_campaign_state_inheritance == 0
AND ngplus_unknown_reward_id_acceptance == 0
AND epilogue_nondeterministic_tie_breaks == 0
AND campaign_epilogue_core_godot_refs == 0
AND campaign_epilogue_core_unity_refs == 0

AND eight_master_archive_integrity == pass
AND archive_blueprint_unlock_tests == pass
AND archive_save_roundtrip == pass
AND archive_recipe_reachability == pass
AND faction_information_provenance == pass

AND captive_60_day_integration == pass
AND captive_escape_determinism == pass
AND captive_oath_atomicity == pass
AND prisoner_save_roundtrip == pass

AND winter_food_tests == pass
AND food_50_plus_cohort_roundtrip == pass
AND food_spoilage_determinism == pass
AND nutrition_medical_boundary == pass
AND sanitation_odor_boundary == pass

AND twelve_epilogue_vignettes_integrity == pass
AND epilogue_16_scenario_branching == pass
AND epilogue_legacy_rating_determinism == pass
AND epilogue_timeline_consistency == pass
AND epilogue_ngplus_validation == pass

AND catalog_integrity == pass
AND content_utilization == pass
AND data_integrity_selftest == pass
AND content_acceptance_gate == pass
AND verify_fast == pass
```

Otherwise: **NO SHIP**.

---

# 33. Implementer Handoff

1. Start with the authority matrix and current API inventory.
2. Add the eight blueprint archives only after every proposed technology has a real Research/TechTree/recipe/project consumer.
3. Keep cleaning chemicals as canonical inventory transactions.
4. Use real scheduler and SkillProgression data for Engineer/Scholar decoding.
5. Keep effort points deterministic and integer/fixed-point where practical.
6. Emit one stable blueprint-discovered event.
7. Let ResearchSystem and TechTree commit the knowledge state.
8. Let recipes/projects become available through authored unlock predicates; never mutate JSON at runtime.
9. Unlock Codex fragments through the Codex authority.
10. Require information propagation before faction reactions.
11. Define physical/or informational archive copy semantics before allowing repeated trade.
12. Route reputation and treaty perks through canonical faction systems.
13. Route unlock fanfare through the acoustic director and keep it optional/headless-safe.
14. Expose technology facts to the epilogue without direct epilogue flags in the archive system.
15. Deepen captive profiles using references to canonical traits/mental-health data rather than copying those values.
16. Implement conspiracy risk from treatment, security, guards, contact, tools, loyalty and rehabilitation—not time alone.
17. Treat 30 days, security 150 and loyalty 800 as tuning values until scale audit is complete.
18. Build the escape attempt as a staged orchestrator over inventory, security, guards and world transitions.
19. Never create virtual stolen tools.
20. Implement vocational rehabilitation through real mentor time, schedule and SkillProgression.
21. Keep formal labor-union mechanics deferred unless the repo already has an organized-labor authority.
22. Make the Oath of the Holdfast a single atomic captive→citizen transition preserving person identity.
23. Ensure a person cannot remain both captive and citizen.
24. Route peer support through mental health and prisoner exchange through diplomacy/faction systems.
25. Prevent hidden cabal audio from leaking secret information.
26. Add smoking/canning tiers as data-driven preservation methods.
27. Verify room/build capability tags rather than scattering room-ID checks.
28. Add smoked-fish and canned-stew recipes only through canonical recipe/item catalogs.
29. Have YearOfAsh and thermal/storage systems determine winter exposure.
30. Have insulation improve the canonical storage model, not food-local temperature.
31. Treat Day 45 as campaign tuning unless it is a guaranteed canonical onset.
32. Keep diet diversity in nutrition and scurvy/malnutrition in medical systems.
33. Route rotten-food odor into sanitation/air exactly once.
34. Validate deterministic save/load with 50+ food cohorts.
35. Build the epilogue as a one-shot derived snapshot over canonical campaign facts.
36. Add 12 vignettes with stable categories, priorities and exclusion rules.
37. Publish the full 0..1000 legacy formula and anti-double-count rules.
38. Derive ethical/governance scoring only from actual recorded actions.
39. Build the timeline from semantic history rather than parsing journal prose.
40. Generate survivor epitaphs/post-war notes from deterministic localized templates.
41. Build a compact versioned NG+ legacy package with whitelisted bounded rewards.
42. Never import prior campaign inventory, survivor objects, or faction state wholesale.
43. Validate shared legacy seeds/checksums and unknown reward IDs.
44. Derive Lords of the Highway, Sanity of the Deep, Technological Golden Age and Abundant Granary from real end-state metrics.
45. Keep all epilogue audio outside Core.
46. Run targeted tests, 60/90/120/180-day simulations, content reachability, save roundtrips, engine-reference scans and `verify-fast`.
47. Produce a final SHIP/NO-SHIP report with every proving command.

---

# 34. Final Outcome

When this plan is complete, ASHFALL’s late game becomes materially deeper without fragmenting its architecture.

Prewar archives stop being isolated lore containers and become a controlled source of transformative knowledge.

A master spool may contain geothermal engineering, hydroponic nutrient chemistry, magnetic propulsion, pulse radar, advanced metallurgy, radiation shielding, water reclamation, or another high-tier technology. But merely finding the spool is not enough.

It may be corroded.
It may require scarce chemicals.
It may need an Engineer and a Scholar working together.
It may take days of real scheduled specialist effort.

When the knowledge is finally reconstructed, the archive does not directly flip a workshop recipe on.

It emits a canonical breakthrough.

Research and the TechTree recognize that breakthrough.
The foundry and workshop then expose recipes whose authored prerequisites are now satisfied.
Construction projects become possible through the same mechanism.
Codex fragments become readable because the Codex authority received a real unlock.

That is knowledge progression rather than a shortcut.

The world can react to it, but only if the world learns about it.

A rival faction does not suddenly know that a reel was decrypted deep underground.
They must hear a rumor, infiltrate the shelter, observe the technology, receive a disclosure, or encounter a traded copy.

Once they know, the archive becomes political.

An envoy can offer to buy a transcription.
A friendly faction can seek a treaty exchange.
A hostile actor can attempt theft.

If the player wants to sell the knowledge repeatedly, they need actual copy/provenance semantics. The same unique physical master spool cannot be sold five times because a dialogue option remained enabled.

Captive management gains the same depth.

Long custody under harsh, insecure conditions can create conspiracy risk, but no magic Day-31 switch creates an escape cabal.

Captives need contact.
They need motive.
They need opportunity.
Weak cell security and negligent guards matter.
Access to workshop tools matters.
Rehabilitation and hope matter.

An escape becomes a chain of real actions: organizing, obtaining tools, breaching containment, moving through the shelter, and reaching the surface. Each stage uses the systems that already own inventory, security, guards and movement.

The positive path becomes equally concrete.

A captive can enter vocational rehabilitation under a shelter craftsperson. That consumes mentor time and captive time. It advances real skills. It can create trust and social integration.

Eventually, an eligible paroled captive may take the Oath of the Holdfast.

At that moment they do not become a “citizen flag” while remaining a prisoner.

The transition is atomic.

The same person leaves captive state and enters canonical citizen/survivor state with their history, skills, relationships and trauma intact.

Prisoner exchange becomes a real ethical and strategic choice too.

Returning an enemy officer can improve diplomatic relations and recover hostages through faction systems.

Refusing does not magically award “labor output.” It simply retains custody, after which any productive contribution still depends on real work assignment and the game’s governance rules.

Food preservation then gives those social and technological systems something to survive for.

Smoking chambers and pressure canners create true long-life reserves.

Fresh fish can become smoked emergency ration.
Vegetables can become canned stew.
Those foods last because the preservation system actually tracks their shelf life.

Winter does not destroy food because a hardcoded calendar day arrived.

YearOfAsh says the severe winter has begun.
The thermal system says the root cellar is freezing.
The storage system says whether the cellar was insulated.
The food system then determines what happens to each cohort.

A player who invested scrap cloth and insulation foam in storage has a real buffer.

A player who ignored winter preparation may lose vulnerable fresh produce.

The nutrition layer can then distinguish a varied preserved diet from surviving exclusively on poor emergency scraps. If the game supports micronutrient deficiencies, the medical system—not FoodPreservation—creates scurvy or malnutrition.

Rotting food can make the shelter dangerous too, but through one path: spoiled cohorts produce an odor/waste observation, sanitation and air systems process it, and health consequences emerge from there.

Finally, the campaign epilogue can make sense of all of this.

It does not maintain a hidden morality meter for 120 days.

At the end, it reads what actually happened.

How many people survived?
How much knowledge was saved?
Were captives rehabilitated, exchanged, abused, or executed?
Did the shelter govern through a council or coercion?
Did the player preserve enough food to withstand winter?
Did survivors recover psychologically?
Did the shelter build an operational vehicle network?
Were master technologies merely discovered, or actually deployed?

Those facts create a transparent Shelter Legacy Rating.

They also select narrative vignettes.

A technologically transformed shelter may reach a **Technological Golden Age**.
A vehicle-centered civilization may become **Lords of the Highway**.
A shelter that preserved deep psychological stability may earn **The Sanity of the Deep**.
A community that enters the postwar era with huge preserved reserves may be remembered for an **Abundant Granary**.

The timeline shows the days that mattered because semantic campaign events actually marked them.

Survivors receive deterministic post-war life notes based on what they lived through.

And a victorious campaign can generate a compact New Game Plus legacy package that carries a bounded heritage into the next run without smuggling the entire old save into a new campaign.

The end result is a genuine legacy loop:

knowledge can transform production;
captives can become conspirators, exchange subjects, apprentices, or citizens;
food preparation can decide whether winter is survivable;
and the epilogue records what those choices made the shelter become.

Every major consequence remains owned by exactly one system.

That is the shipping standard for C1 Plan Integration [45].
